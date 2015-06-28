using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class CommentService: BaseService, ICommentService
    {
        public CommentService(string webServiceAddress) : base(webServiceAddress)
        {

        }
    }
}
