using PrivateTraining.ServiceLayer.Interface.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DataLayer.Context;
using System.Data.Entity;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace PrivateTraining.ServiceLayer.Repository.Framework
{
    public class MessageRepository : IMessage
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<Message> _message;
        private readonly IIdentity _identity;

        public MessageRepository(IUnitOfWork uow, IIdentity identity)
        {
            _uow = uow;
            _message = _uow.Set<Message>();
            _identity = identity;

        }

        public async Task AddMessage(Message Model)
        {
            _message.Add(Model);
        }

        public IQueryable<Message> GetAllMessage(int MessageId=0)
        {
            if (MessageId != 0)
                return _message.Where(c => c.IsEnable == true && c.Id == MessageId);
            else
                return _message.Where(c => c.IsEnable == true);
        }

        public void DeleteMessage(int MesseageId)
        {
            var Query = _message.Find(MesseageId);
            Query.IsEnable = false;
            var Status = _uow.SaveAllChanges();
        }

        public int GetCountUserMessage()
        {
            var UserId = _identity.GetUserId<int>();

            return _message.Where(x => x.IsEnable==true && x.ReciverUserId== UserId && x.ReadMessage==false).Count();

        }
    }

}
