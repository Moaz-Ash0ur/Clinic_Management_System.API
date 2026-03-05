using Asp.Versioning;
using ClinicManagement.API.Controllers;
using ClinicManagement.Application.Featuers.Payments.Command.HandleWebhook;
using ClinicManagement.Application.Featuers.Payments.Command.InitializePayment;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/v{version:apiVersion}/payments")]
[ApiVersion("1.0")]
public class PaymentController : ApiController
{
    private readonly ISender _sender;

    public PaymentController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost("InitializePayment", Name = "InitializePayment")]
    [ProducesResponseType(typeof(PaymentRedirectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Initializes a payment process.")]
    [EndpointDescription("Creates a payment and returns a redirect URL to complete the payment.")]
    [EndpointName("InitializePayment")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> CreatePayment(
        [FromQuery] Guid invoiceId,
        [FromQuery] PaymentMethod method)
    {

        var command = new InitializePaymentCommand(invoiceId, method);

        var result = await _sender.Send(command);

        return result.Match(
            response =>
            {
                return Ok(response);
            },
            Problem);
    }



    [HttpPost("webhook", Name = "HandlePaymentWebhook")]
    [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Handles payment gateway webhook callback.")]
    [EndpointDescription("Processes server-to-server webhook notifications sent by the payment gateway.")]
    [EndpointName("HandlePaymentWebhook")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> ServerCallback([FromBody] JsonElement payload)
    {
        var receivedHmac = Request.Query["hmac"].ToString();


        var command = new HandleWebhookCommand(payload, receivedHmac);

        var result = await _sender.Send(command);

        return result.Match(
            response =>
            {
                return Ok(response);
            },
            Problem);
    }





}