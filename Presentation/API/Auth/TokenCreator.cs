using API.Models.Accounts;
using API.Models.AppSetting;
using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Framework.Controllers;
using Tm.Services.Pms.MasterData;

namespace API.Auth
{
	public class TokenCreator : BaseController
	{
		private readonly AD _ADconfig;
		private readonly IMasterDataService _masterdataService;
		public TokenCreator(IOptions<AD> ADconfig, IMasterDataService masterdataService)
		{
			_ADconfig = ADconfig.Value;
			_masterdataService = masterdataService;
		}

		//[HttpPost]
		//[Route("RefreshToken")]
		//public async Task<BaseResponseModel> RefreshToken(string code, string Url)
		//{
		//	string endPoint = _ADconfig.EndPointURL;
		//	var client = new HttpClient();

		//	var data = new[]
		//	{
		//		 new KeyValuePair<string, string>(ConstantValues.ClientId, _ADconfig.ClientId),
		//		 new KeyValuePair<string, string>(ConstantValues.Scope, _ADconfig.Scope),
		//		  new KeyValuePair<string, string>(ConstantValues.RedirectUri, string.IsNullOrEmpty(Url) ? _ADconfig.RedirectUri : Url),
		//		   new KeyValuePair<string, string>(ConstantValues.GrantType, _ADconfig.GrantType),
		//		   new KeyValuePair<string, string>(ConstantValues.ClientSecret, _ADconfig.ClientSecret),
		//			 new KeyValuePair<string, string>(ConstantValues.Code, code),
		//   };
		//	var response = client.PostAsync(endPoint, new FormUrlEncodedContent(data)).GetAwaiter().GetResult();
		//	var token = await response.Content.ReadAsStringAsync();
		//	var result = JsonSerializer.Deserialize<Token>(token);
		//	var adUserId = await GetIdTokenUserId(result.access_token);
		//	result.userId = adUserId;
		//	var refreshTokenUser = await _masterdataService.GetRefreshTokenByUserId(adUserId);
		//	if (refreshTokenUser == null)
		//	{
		//		RefreshToken refreshToken = new RefreshToken();
		//		refreshToken.UserId = adUserId;
		//		refreshToken.Token = result.refresh_token;
		//		await _masterdataService.InsertRefreshToken(refreshToken);
		//	}
		//	else
		//	{
		//		refreshTokenUser.UserId = adUserId;
		//		refreshTokenUser.Token = result.refresh_token;
		//		await _masterdataService.InsertRefreshToken(refreshTokenUser);
		//	}
		//	return new BaseResponseModel() { StatusCode = 200, Data = result, Message = ConstantValues.Success };
		//}
		[HttpPost]
		[Route("GetToken")]
		public async Task<BaseResponseModel> GetToken(string code, string Url)
		{
			string endPoint = _ADconfig.EndPointURL;
			var client = new HttpClient();

			var data = new[]
			{
				 new KeyValuePair<string, string>(ConstantValues.ClientId, _ADconfig.ClientId),
				 new KeyValuePair<string, string>(ConstantValues.Scope, _ADconfig.Scope),
				  new KeyValuePair<string, string>(ConstantValues.RedirectUri, string.IsNullOrEmpty(Url) ? _ADconfig.RedirectUri : Url),
				   new KeyValuePair<string, string>(ConstantValues.GrantType, _ADconfig.GrantType),
				   new KeyValuePair<string, string>(ConstantValues.ClientSecret, _ADconfig.ClientSecret),
					 new KeyValuePair<string, string>(ConstantValues.Code, code),
		   };
			var response = client.PostAsync(endPoint, new FormUrlEncodedContent(data)).GetAwaiter().GetResult();
			var token = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<Token>(token);
			var adUserId = await GetIdTokenUserId(result.access_token);
			result.userId = adUserId;
			return new BaseResponseModel() { StatusCode = 200, Data = result, Message = ConstantValues.Success };
		}
		[HttpGet]
		[Route("GetAllADUsers")]
		public async Task<BaseResponseModel> GetAllADUsers(string token)
		{
			string jsonResponse = "";
			string endPoint = _ADconfig.AllUserEndPoint;
			var query = new Dictionary<string, string>()
			{
				[ConstantValues.AdCount] = _ADconfig.Count
			};
			var uri = QueryHelpers.AddQueryString(endPoint, query);
			var request = new HttpRequestMessage(HttpMethod.Get, uri);
			request.Headers.Authorization = new AuthenticationHeaderValue(ConstantValues.Bearer, token);
			var client = new HttpClient();
			HttpResponseMessage response = await client.SendAsync(request);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				jsonResponse = await response.Content.ReadAsStringAsync();
			}
			var res = JsonSerializer.Deserialize<Root>(jsonResponse);
			return new BaseResponseModel() { StatusCode = 200, Data = res, Message = ConstantValues.Success };
		}

		private async Task<string> GetIdTokenUserId(string idtoken)
		{
			var token = new JwtSecurityToken(jwtEncodedString: idtoken);
			string userId = token.Claims.First(c => c.Type == "oid").Value;
			return userId;
		}
	}
}
