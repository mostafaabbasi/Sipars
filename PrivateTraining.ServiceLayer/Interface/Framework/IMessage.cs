using PrivateTraining.DomainClasses.Entities.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.Framework
{
    public interface IMessage
    {
        IQueryable<Message> GetAllMessage(int MessageId);
        Task AddMessage(Message Model);
        void DeleteMessage(int MesseageId);
        int GetCountUserMessage();

    }
}
