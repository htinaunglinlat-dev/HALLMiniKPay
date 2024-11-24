using System.Reflection;
using HALLMiniKPay.Database.Models;
using HALLMiniKPay.RestApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HALLMiniKPay.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private AppDbContext _db = new AppDbContext(); 
        [HttpGet("test/readAllWallets")]
        public IActionResult GetTestApi() {
            var wallet = _db.TblWallets.ToList();
            return Ok(wallet);
        }
        [HttpPost("test/createFakeTransition")]
        public IActionResult PostTransition()
        {
            _db.TblTransitions.Add(new TblTransition
            {
                FromPhone = "0911111",
                ToPhone = "0912345",
                Amount = 10000,
                Note = "pocket money",
                Date = DateTime.Now
            });
            int result = _db.SaveChanges();
            if(result == 0)
            {
                return StatusCode(500,"Failed to create mock transition.");
            } else
            {
                return Ok("Successfully Created Fake Transition");
            }
        }
        [HttpPost("checkBalance")]
        public IActionResult CheckBalance(CheckBalanceViewModel model)
        {
            if (String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Pin))
            {
                return BadRequest("Authentication field are required.");
            }
            var item = _db.TblWallets.AsNoTracking().FirstOrDefault(x => x.Phone == model.Phone);
            if (item is null)
            {
                return NotFound("Invalid Phone number.");
            }
            if (item.Pin != model.Pin)
            {
                return BadRequest("Uncorrected Authentication.");
            }
            return Ok($"username: {item.Username}\n phone no: {item.Phone}\n Balance: {item.Amount}");
        }
        [HttpPost("checkTransitionHistory")]
        public IActionResult CheckTransitionHistory(AuthenticationViewModel model)
        {
            if(String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Pin)) {
                return BadRequest("Authentication field are required.");
            }
            var user = _db.TblWallets.AsNoTracking().FirstOrDefault(x => x.Phone == model.Phone);
            if(user is null)
            {
                return NotFound("Invalid Phone number.");
            }
            if (user.Pin != model.Pin)
            {
                return BadRequest("Uncorrected Authentication.");
            }
            var lst = _db.TblTransitions.AsNoTracking().Where(x => x.FromPhone == model.Phone).ToList();
            return Ok(lst);
        }
        [HttpPost("createUser")]
        public IActionResult CreateUserAccount(CreateUserViewModel model)
        {
            if (String.IsNullOrEmpty(model.Username) || String.IsNullOrEmpty(model.Phone))
            {
                return BadRequest("All fields are required.");
            }
            if(model.Amount < 10000)
            {
                return BadRequest("Creation accounts need at least 10000 at wallet.");
            }
            if(model.Pin.Length < 4 || model.Pin.Length > 10)
            {
                return BadRequest("Pin length must be between 4 to 10");
            }
            _db.TblWallets.Add(new TblWallet
            {
                Phone = model.Phone,
                Username = model.Username,
                Amount = model.Amount,
                Pin = model.Pin
            });
            int result = _db.SaveChanges();
            if(result == 0)
            {
                return StatusCode(500, "failed at creation new user account.");
            } else
            {
                return Ok("Successfully created.");
            }

        }
        [HttpPatch("withdraw")]
        public IActionResult Withdraw(WithdrawAndDepositViewModel model)
        {
            if(String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Pin))
            {
                return BadRequest("All fields are required.");
            }
            if(model.Amount <= 0)
            {
                return BadRequest("The transfer amount cannot be 0 and less than 0.");
            }
            var item = _db.TblWallets.AsNoTracking().FirstOrDefault(x => x.Phone == model.Phone);
            if(item is null)
            {
                return NotFound("Invalid Phone number.");
            }
            if(item.Pin != model.Pin)
            {
                return BadRequest("Uncorrected Authentication.");
            }
            if ((item.Amount - 10000) < Math.Abs(model.Amount))
            {
                return BadRequest("Less amount than the atleast amount (10000). ");
            }
            item.Amount -= Math.Abs(model.Amount);
            _db.Entry(item).State = EntityState.Modified;
            int result = _db.SaveChanges();
            if(result == 0)
            {
                return StatusCode(500, "Unsuccess withdraw.");
            }
            return Ok("Successfully Withdraw.");
        }
        [HttpPatch("deposit")]
        public IActionResult Deposit(WithdrawAndDepositViewModel model)
        {
            if (String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Pin))
            {
                return BadRequest("Authentication field are required.");
            }
            if (model.Amount <= 0)
            {
                return BadRequest("The transfer amount cannot be 0 and less than 0.");
            }
            var item = _db.TblWallets.AsNoTracking().FirstOrDefault(x => x.Phone == model.Phone);
            if (item is null)
            {
                return NotFound("Invalid Phone number..");
            }
            if (item.Pin != model.Pin)
            {
                return BadRequest("Uncorrected Authentication.");
            }
            item.Amount += Math.Abs(model.Amount);
            _db.Entry(item).State = EntityState.Modified;
            int result = _db.SaveChanges();
            if (result > 0)
            {
                return Ok("Successfully Deposited");
            }
            else
            {
                return StatusCode(500, "Unsuccess deposited.");
            }
        }
        [HttpPatch("transfer")]
        public IActionResult Transfer(TransferViewModel model)
        {
            if (String.IsNullOrEmpty(model.FromPhone) || String.IsNullOrEmpty(model.ToPhone) || String.IsNullOrEmpty(model.Pin) || String.IsNullOrEmpty(model.Note))
            {
                return BadRequest("All fields are required.");
            }
            if (model.FromPhone == model.ToPhone)
            {
                return BadRequest("sender and receiver phone number cannot be the same.");
            }
            if (model.Amount <= 0)
            {
                return BadRequest("The transfer amount cannot be 0 and less than 0.");
            }
            var sender = _db.TblWallets.AsNoTracking().FirstOrDefault(x => x.Phone == model.FromPhone);
            if (sender is null)
            {
                return NotFound("Invalid Phone number.");
            }
            if (sender.Pin != model.Pin)
            {
                return BadRequest("Uncorrected Authentication.");
            }
            var receiver = _db.TblWallets.AsNoTracking().FirstOrDefault(x => x.Phone == model.ToPhone);
            if (receiver is null)
            {
                return NotFound($"Invalid Receiver Phone number. phone no : {model.ToPhone}");
            }
            //Console.WriteLine($"Math.Abs(model.Amount) - 10000 {Math.Abs(model.Amount) - 10000}");
            //Console.WriteLine($"sender.Amount {sender.Amount}");
            //Console.WriteLine((Math.Abs(model.Amount) - 10000) < sender.Amount);
            if((Math.Abs(sender.Amount) - 10000) < model.Amount)
            {
                return BadRequest("Less amount than the atleast amount (10000).");
            }
            sender.Amount -= Math.Abs(model.Amount);
            _db.Entry(sender).State = EntityState.Modified;
            int senderResult = _db.SaveChanges();
            if (senderResult == 0)
            {
                return StatusCode(500, "Unsuccess transition at sender account.");
            }
            receiver.Amount += Math.Abs(model.Amount);
            _db.Entry(receiver).State = EntityState.Modified;
            int receiverResult = _db.SaveChanges();
            if (receiverResult == 0) {
                return StatusCode(500, "Unsuccess transition at receiver account.");
            }
            _db.TblTransitions.Add(new TblTransition
            {
               FromPhone = sender.Phone,
               ToPhone = receiver.Phone,
               Amount = Math.Abs(model.Amount),
               Note = model.Note,
               Date = DateTime.Now
            });
            int transitionResult = _db.SaveChanges();
            if(transitionResult == 0)
            {
                return StatusCode(500, "Failed to create the transition");
            }
            return Ok($"Successfully Transferred.\nFrom: { model.FromPhone}\nTo: { model.ToPhone}\nAmount: { model.Amount}\nNote: { model.Note}");
        }
    }
}
