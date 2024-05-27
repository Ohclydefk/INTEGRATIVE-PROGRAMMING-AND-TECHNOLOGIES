//using Microsoft.AspNetCore.Mvc;
//using RestSharp;
//using System.Threading.Tasks;

//namespace IT15_Project.Controllers.API
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PaymongoControllerCopy : ControllerBase
//    {
//        [HttpPost("create-payment-link")]
//        public async Task<IActionResult> CreatePaymentLink()
//        {
//            try
//            {
//                // Set fixed values for amount, currency, and description
//                decimal amount = 150000; // Fixed amount of 1500 PHP
//                string currency = "PHP";
//                string description = "Appointment Payment";

//                var options = new RestClientOptions("https://api.paymongo.com/v1/links");
//                var client = new RestClient(options);
//                var request = new RestRequest("", Method.Post);

//                request.AddHeader("accept", "application/json");
//                request.AddHeader("content-type", "application/json");
//                request.AddHeader("authorization", "Basic c2tfdGVzdF9wOW1rV1lYc0NVbUJXMlRLWDE0WVBxUFc6");

//                // Create request payload with fixed values
//                var requestModel = new PaymentLinkRequest
//                {
//                    Data = new PaymentLinkData
//                    {
//                        Attributes = new PaymentLinkAttributes
//                        {
//                            Amount = amount,
//                            Currency = currency,
//                            Description = description
//                        }
//                    }
//                };

//                request.AddJsonBody(requestModel);

//                var response = await client.ExecuteAsync(request);

//                if (response.IsSuccessful)
//                {
//                    return Ok(response.Content);
//                }
//                else
//                {
//                    return StatusCode((int)response.StatusCode, response.Content);
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle error
//                return StatusCode(500, $"An error occurred: {ex.Message}");
//            }
//        }

//    }

//    // Define models to represent the request payload
//    public class PaymentLinkRequest
//    {
//        public PaymentLinkData? Data { get; set; }
//    }

//    public class PaymentLinkData
//    {
//        public PaymentLinkAttributes? Attributes { get; set; }
//    }

//    public class PaymentLinkAttributes
//    {
//        public decimal Amount { get; set; }
//        public string? Currency { get; set; }
//        public string? Description { get; set; }
//    }
//}
