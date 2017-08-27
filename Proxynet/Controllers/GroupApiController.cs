using System;
using System.Collections.Generic;
using System.Web.Http;
using Proxynet.Models;
using Proxynet.Service.Finders;
using Proxynet.Service.Removers;
using Proxynet.Service.Savers;

namespace Proxynet.Controllers
{
    [RoutePrefix("service/groups")]
    public class GroupApiController : BaseApiController
    {
        private readonly IGroupSaver _groupSaver;
        private readonly IGroupDataFinder _groupDataFinder;
        private readonly IDataRemover _dataRemover;

        public GroupApiController(IGroupSaver groupSaver, IGroupDataFinder groupDataFinder, IDataRemover dataRemover)
        {
            _groupSaver = groupSaver;
            _groupDataFinder = groupDataFinder;
            _dataRemover = dataRemover;
        }

        [HttpPut]
        [Route("save-group")]
        public IHttpActionResult SaveGroup(GroupDto group)
        {
            if (String.IsNullOrEmpty(group.Name))
            {
                return NotFound();
            }
            GroupDto groupDto = _groupSaver.SaveGroup(group);

            return Ok(groupDto);
        }
        

        [HttpGet]
        [Route("get-groups")]
        public IHttpActionResult GetGroups()
        {
            List<GroupDto> userGroupsDto = _groupDataFinder.GetGroupsDto();
            return Ok(userGroupsDto);
        }

        [HttpDelete]
        [Route("delete-group")]
        public IHttpActionResult DeleteGroup(int groupId)
        {
            _dataRemover.RemoveGroup(groupId);
            return Ok();
        }
    }
}