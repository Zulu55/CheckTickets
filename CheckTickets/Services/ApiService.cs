using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CheckTickets.Models;
using Newtonsoft.Json;

namespace CheckTickets.Services
{
    public class ApiService
	{
		public async Task<Response> Login(string urlBase, string servicePrefix,
										  string controller, string email,
                                          string password)
		{
			try
			{
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password,
                };

				var request = JsonConvert.SerializeObject(loginRequest);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.PostAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var user = JsonConvert.DeserializeObject<User>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "Login OK",
                    Result = user,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> GetTicket(string urlBase, 
                                              string servicePrefix,
										      string controller, 
                                              string ticketCode)
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, 
                                        ticketCode);
				var response = await client.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						//TODO: Mejorar el mensaje cuando falle
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var ticket = JsonConvert.DeserializeObject<Ticket>(result);
				return new Response
				{
					IsSuccess = true,
					Message = "Ok",
					Result = ticket,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Get<T>(string urlBase, string servicePrefix, 
                                           string controller)
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
                        //TODO: Mejorar el mensaje cuando falle
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var model = JsonConvert.DeserializeObject<T>(result);
				return new Response
				{
					IsSuccess = true,
					Message = "Ok",
					Result = model,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> GetList<T>(string urlBase, string servicePrefix,
										   string controller)
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						//TODO: Mejorar el mensaje cuando falle
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var model = JsonConvert.DeserializeObject<List<T>>(result);
				return new Response
				{
					IsSuccess = true,
					Message = "Ok",
					Result = model,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Post<T>(string urlBase, string servicePrefix, 
                                            string controller, T model)
		{
			try
			{
				var request = JsonConvert.SerializeObject(model);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.PostAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var newRecord = JsonConvert.DeserializeObject<T>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "Record added OK",
					Result = newRecord,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Put<T>(string urlBase, string servicePrefix, 
                                           string controller, T model)
		{
			try
			{
				var request = JsonConvert.SerializeObject(model);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
				var response = await client.PutAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var newRecord = JsonConvert.DeserializeObject<T>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "Record updated OK",
					Result = newRecord,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Delete<T>(string urlBase, string servicePrefix, 
                                              string controller, T model)
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
				var response = await client.DeleteAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				return new Response
				{
					IsSuccess = true,
					Message = "Record deleted OK",
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}
	}
}