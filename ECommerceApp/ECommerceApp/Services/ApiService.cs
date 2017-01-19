﻿using ECommerceApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Services
{
    public class ApiService
    {
        public async Task<Response> Login(string email, string password)
        {
            try
            {
                var loginRequest = new LoginRequest { Email = email, Password = password };

                var request = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://zulu-software.com");
                    var url = "/Ecommerce/api/Users/Login";
                    var response = await client.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Usuario o contraseña incorrectos"
                        };
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(result);

                    return new Response
                    {
                        IsSuccess = true,
                        Message = "Mensaje oK",
                        Result = user
                    };


                }

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<T>> Get<T>(string controller) where T : class
        {
            try
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://zulu-software.com");
                    var url = $"/Ecommerce/api/{controller}/";

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<T>>(result);

                    return list;
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<Response> NewCustomer(Customer customer)
        {

            try
            {
                var request = JsonConvert.SerializeObject(customer);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://zulu-software.com");
                    var url = "/Ecommerce/api/Customers";
                    var response = await client.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = response.StatusCode.ToString() 
                        };
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    var newcustomer = JsonConvert.DeserializeObject<Customer>(result);

                    return new Response
                    {
                        IsSuccess = true,
                        Message = "Cliente creado oK",
                        Result = newcustomer
                    };

                }

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }
    }
}
