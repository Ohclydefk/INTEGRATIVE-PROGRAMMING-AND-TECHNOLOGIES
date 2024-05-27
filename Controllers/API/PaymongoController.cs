using IT15_Project.Areas.Identity.Data;
using IT15_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace IT15_Project.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymongoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PaymongoController(AppDbContext context)
        {
            _context = context;

        }

        [HttpPost("create-checkout-session")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCheckoutSession([FromForm] Appointment formData, [FromForm] Appointment apiData)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized(new { error = "You must be logged in to make an appointment." });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Appointments.Add(formData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Return validation errors if model state is not valid
                    return BadRequest(ModelState);
                }

                // Call the API
                var options = new RestClientOptions("https://api.paymongo.com/v1/checkout_sessions");
                var client = new RestClient(options);
                var request = new RestRequest("");

                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Basic c2tfdGVzdF9wOW1rV1lYc0NVbUJXMlRLWDE0WVBxUFc6");

                var billingInfo = new
                {
                    name = apiData.FullName,
                    email = apiData.Email,
                    phone = apiData.PhoneNumber
                };

                string jsonBody = $@"
        {{
            ""data"": {{
                ""attributes"": {{
                    ""billing"": {JsonConvert.SerializeObject(billingInfo)},
                    ""send_email_receipt"": false,
                    ""show_description"": true,
                    ""show_line_items"": true,
                    ""payment_method_types"": [""gcash"", ""card"", ""paymaya""],
                    ""description"": ""Appointment Payment"",
                    ""line_items"": [
                        {{
                            ""currency"": ""PHP"",
                            ""amount"": 250000,
                            ""description"": ""Payment for appointment"",
                            ""quantity"": 1,
                            ""name"": ""Appointment Package""
                        }}
                    ]
                }}
            }}
        }}";

                request.AddJsonBody(jsonBody.ToString(), false);

                var response = await client.PostAsync(request);

                if (response.IsSuccessful)
                {
                    return Ok(response.Content);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.Content);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}" });
            }
        }







        [HttpPost("create-payment-intents")]
        public async Task<IActionResult> CreatePaymentIntent()
        {
            try
            {
                decimal amount = 150000;
                string currency = "PHP";
                string description = "Appointment Payment";
                string[] payment_method_allowed = { "gcash", "grab-pay" };
                string capture_type = "automatic";
                string request_three_d_secure = "any";

                string jsonBody = $@"
                {{
                    ""data"": {{
                        ""attributes"": {{
                            ""amount"": {amount},
                            ""currency"": ""{currency}"",
                            ""description"": ""{description}"",
                            ""payment_method_allowed"": [""{string.Join("\", \"", payment_method_allowed)}""],
                            ""payment_method_options"": {{
                                ""card"": {{
                                    ""request_three_d_secure"": ""{request_three_d_secure}""
                                }}
                            }},
                            ""capture_type"": ""{capture_type}""
                        }}
                    }}
                }}";

                var options = new RestClientOptions("https://api.paymongo.com/v1/payment_intents");
                var client = new RestClient(options);
                var request = new RestRequest("", Method.Post);

                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Basic c2tfdGVzdF9wOW1rV1lYc0NVbUJXMlRLWDE0WVBxUFc6");

                request.AddJsonBody(jsonBody, false);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    return Ok(response.Content);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.Content);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}" });
            }
        }

    }
}
