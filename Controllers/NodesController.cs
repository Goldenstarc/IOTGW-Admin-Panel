using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using IOTGW_Admin_Panel.Models;
using IOTGW_Admin_Panel.Services;
using IOTGW_Admin_Panel.Helpers;
using AutoMapper;

namespace IOTGW_Admin_Panel.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private INodeService _nodeService;
        private IMapper _mapper;

        public NodesController(DataBaseContext context, INodeService nodeService, IMapper mapper)
        {
            _context = context;
            _nodeService = nodeService;
            _mapper = mapper;
        }

        /// <summary>
        /// gets nodes list.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Node>> GetAll()
        {
            try
            {
                var nodes = _nodeService.GetAll();
                var nodeMap = _mapper.Map<IList<Node>>(nodes);
                return Ok(nodeMap);
            }
            catch (AppException ex)
            {
                return NotFound(ex.Message);
            }

        }

        /// <summary>
        /// gets a specific node.
        /// </summary>
        /// <param id="Id"></param> 
        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult<Node> GetById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            try
            {
                var node = _nodeService.GetById(id);

                // if (node.Gateway.UserId != currentUserId && !User.IsInRole(Role.Admin))
                //     return Forbid();

                var node2 = _nodeService.GetById(id);
                var nodeMap = _mapper.Map<Node>(node2);
                return Ok(nodeMap);
            }
            catch (AppException ex)
            {
                return NotFound(ex.Message);
            }

        }

        /// <summary>
        /// gets messages list for a node.
        /// </summary>
        [HttpGet]
        [Route("{nodeId:int}/messages")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<NodeDto>> GetMessagesForNode(int nodeId)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            try
            {
                var node = _nodeService.ClaimCheck(nodeId);
                if (node.Gateway.UserId != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                var messages = _nodeService.GetMessagesForNode(nodeId);
                var messagesDtoMap = _mapper.Map<IList<MessageDto>>(messages);
                return Ok(messagesDtoMap);
            }
            catch (AppException ex)
            {
                return NotFound(ex.Message);
            }

        }

        /// <summary>
        /// Ctreate new node.
        /// </summary>
        /// <param node="node Item"></param> 
        [Authorize(Roles = Role.Admin)]
        //[HttpPost("register")]
        [HttpPost]
        [Produces("application/json")]
        public IActionResult Create(Node nodeParam)
        {
            var nodeMap = _mapper.Map<Node>(nodeParam);
            try
            {
                _nodeService.Create(nodeMap);
                return CreatedAtAction(nameof(GetById), new { id = nodeMap.Id }, nodeMap);
                //return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a specific node.
        /// </summary>
        /// <param name="id"></param>   
        /// <param node="node Item"></param>   
        [HttpPut("{id}")]
        public IActionResult Update(int id, Node nodeParam)
        {

            //only allow admins to access other node records
            var currentUserId = int.Parse(User.Identity.Name);

            // if (nodeParam.Gateway.UserId != currentUserId && !User.IsInRole(Role.Admin))
            //     return Forbid();

            var nodeMap = _mapper.Map<Node>(nodeParam);
            nodeMap.Id = id;

            try
            {
                var node = _nodeService.ClaimCheck(id);
                if (node.Gateway.UserId != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                _nodeService.Update(nodeMap);
                return NoContent();
                //return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Deletes a specific node.
        /// </summary>
        /// <param name="id"></param>   
        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Delete(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            try
            {
                var node = _nodeService.GetById(id);

                _nodeService.Delete(id);
                return NoContent();
                //return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

    }
}