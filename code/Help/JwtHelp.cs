using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Help
{
    public class JwtHelp
    {
        private static string SecretKey = "85851d5c4720465b9d45be53e2c2b250";
        private static string JWTAudience = "user";
        private static string JWTIssuer = "wanghu";

        /// <summary>
        /// 获取JWT
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="expTime">过期时间 单位分钟</param>
        /// <returns></returns>
        public static string CreateJWT(Dictionary<string, string> dic, int expTime)
        {
            Claim[] claims = new Claim[dic.Count()];
            int i = 0;
            foreach (var item in dic)
            {
                Claim claim = new Claim(item.Key, item.Value.ToString());
                claims[i] = claim;
                i++;
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(JWTIssuer, JWTAudience, claims, DateTime.Now, DateTime.Now.AddMinutes(expTime), creds);
            string strToken = new JwtSecurityTokenHandler().WriteToken(token);
            return strToken;
        }

        /// <summary>
        /// 解密JWT
        /// </summary>
        /// <param name="token"></param>
        public static JwtPayload DecryptJWT(string securityToken)
        {
            JwtPayload payload = null;
            try
            {
                var token = new JwtSecurityToken(securityToken);
                payload = token.Payload;
                var role = (from t in payload where t.Key == ClaimTypes.Role select t.Value).FirstOrDefault();
                var name = (from t in payload where t.Key == ClaimTypes.Name select t.Value).FirstOrDefault();
                //var issuer = token.Issuer;
                //var key = token.SecurityKey;
                //var audience = token.Audiences;
                //var identity = new ClaimsIdentity();
                //identity.AddClaim(new Claim(ClaimTypes.Name, name.ToString()));
                //identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
                return payload;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
