using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
	public class MessageService : BaseRepositry<clsMeseges>
	{
		private readonly ApplicationDBContext _dbContext;
		public MessageService(ApplicationDBContext context) : base(context)
		{
			_dbContext = context;
		}
		public async Task<clsMeseges> GetByID(int ID) => await _dbContext.Set<clsMeseges>().FirstOrDefaultAsync(x => x.ID == ID);


		public async Task<List<dynamic>> GetMessegesDetils()
		{
            var query = from user in _dbContext.Users
                        join message in _dbContext.Messages
                        on user.ID equals message.UserID
                        select new
                        {
                            MessageID = message.ID,
                            UserName = user.Name,
                            PurchasesNumber = user.Purchasesnumber,
                            UserImageURL = user.ImageURL,
                            UserMessage = message.Message,
                            Date = message.Date
                        };
            return await query.ToListAsync<object>();
        }

    }
}
