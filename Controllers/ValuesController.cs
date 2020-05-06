using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [Route("api/values")]
    [ApiController]
    //[Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly WebApiContext db;
        public ValuesController(WebApiContext DB)
        {
            db = DB;
        }
        //GET api/values
        [HttpGet]
        //[Authorize(Policy = "Must Be Admin")]
        public ActionResult Get()
        {
            //using (var db = new WebApiContext())
            //{
            //var userDetail = db.Accounts.Where(x => x.UserName == "Khang").FirstOrDefault();
            //if (userDetail == null)
            //    return NotFound();
            //bool validPassword = BCrypt.Net.BCrypt.Verify("Xuankhang123", userDetail.UserPass);
            //if (validPassword)
            //{
            //    var identity = new ClaimsIdentity(new[] {
            //    new Claim(ClaimTypes.Role, userDetail.UserRole),
            //        new Claim(ClaimTypes.Name,userDetail.UserName)},
            //        CookieAuthenticationDefaults.AuthenticationScheme);
            //    var principal = new ClaimsPrincipal(identity);
            //    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //}
            var temp = db.Accounts.ToList();
            return Ok(temp);
        }
        [HttpGet]
        //[Authorize(Policy = "Must Be Admin")]
        [Route("GetEquips")]
        public ActionResult<IEnumerable<Equips>> GetEquips()
        {
            //var temp = db.Equips.ToList();
            //return temp;
            var result = from a in db.Equips
                         join b in db.Types on a.TypeID equals b.TypeID
                         select new EquipInfo { EquipName = a.EquipName, EquipDes = a.EquipDes, EquipStatus = a.EquipStatus, TypeName = b.TypeName, EquipID = a.EquipID };
            var temp = new ListAllInfo();
            temp.EquipInfos = result.ToList();
            return Ok(temp);
        }
        [HttpGet]
        [Route("GetType")]
        public ActionResult<IEnumerable<Models.Type>> GetModelType()
        {
            var temp = db.Types.ToList();
            return temp;
        }
        [HttpGet]
        [Route("GetList")]
        public ActionResult<IEnumerable<List>> GetList()
        {
            var temp = db.Lists.ToList();
            return temp;
        }
        // GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}
        [HttpGet("{id}")]
        [Route("GetAccount/{id}")]
        public IActionResult Get(int id)
        {
            var test = from a in db.Accounts
                       join b in db.Lists on a.UserID equals b.UserID
                       join c in db.Equips on b.EquipID equals c.EquipID
                       join d in db.Types on c.TypeID equals d.TypeID
                       where a.UserID == id
                       select new AllInfo { UserName = a.UserName, EquipName = c.EquipName, EquipDes = c.EquipDes, TypeName = d.TypeName };
            return Ok(test);
        }
        [HttpGet("{id}")]
        [Route("GetEquips/{id}")]
        public  IActionResult GetEquips([FromRoute] int id)
        {
            var test = from a in db.Equips
                       join b in db.Types on a.TypeID equals b.TypeID
                       where a.EquipID == id
                       select new EquipInfo { EquipName = a.EquipName, EquipDes = a.EquipDes, EquipStatus = a.EquipStatus, TypeName = b.TypeName , EquipID = a.EquipID,TypeID = a.TypeID };
            //var equips = await db.Equips.FindAsync(id);
            return Ok(test.SingleOrDefault());
        }


        // POST api/values
        [HttpPost]
        [Route("CreateEquip")]
        public IActionResult Post([FromBody] Equips equip)
        {
            if (ModelState.IsValid)
            {
                db.Add(equip);
                db.SaveChanges();
                return CreatedAtAction("GetEquips", new { id = equip.EquipID }, equip);
            }
            return BadRequest(ModelState);
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        [Route("EditEquip/{id}")]
        public IActionResult Put(int id, [FromBody] Equips value)
        {
            //Khong can ID
            //var selectEquip = db.Equips.FirstOrDefault(x => x.EquipID == id);
            //selectEquip.EquipName = value.EquipName;
            //selectEquip.EquipDes = value.EquipDes;
            //selectEquip.EquipStatus = value.EquipStatus;
            //db.SaveChanges();
            //return Ok(selectEquip);


            // can nhap id
            if (ModelState.IsValid)
            {
                db.Entry(value).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(value);
            }
            return BadRequest(ModelState);
        }


        //DELETE api/values/5
        [HttpDelete("{id}")]
        [Route("DeleteEquip/{id}")]
        public IActionResult Delete(int id)
        {
            var equip = db.Equips.FirstOrDefault(x => x.EquipID == id);
            if (equip != null)
            {
                db.Remove(equip);
                db.SaveChanges();
                return Ok(equip);
            }
            return NotFound();
        }
    }
}
