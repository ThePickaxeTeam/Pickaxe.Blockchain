using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Extensions;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController(INodeService nodeService)
        {
            NodeService = nodeService;
        }

        protected INodeService NodeService { get; private set; }
    }
}
