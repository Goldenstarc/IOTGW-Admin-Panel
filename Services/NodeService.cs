using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using IOTGW_Admin_Panel.Helpers;
using IOTGW_Admin_Panel.Models;

namespace IOTGW_Admin_Panel.Services
{
    public interface INodeService
    {
        // Dont use Task<> Cause cant catch : throw new AppException
        IEnumerable<Node> GetAll();
        Node GetById(int id);
        Node Create(Node node);
        void Update(Node node);
        void Delete(int id);
    }

    public class NodeService : INodeService
    {
        private readonly DataBaseContext _context;

        public NodeService(DataBaseContext context)
        {
            _context = context;
        }
        public IEnumerable<Node> GetAll()
        {
            if (!_context.Nodes.Any())
                throw new AppException("No Node");

            //return _context.Nodes.Include(x => x.Gateway).ToList();
            return _context.Nodes.ToList();

        }
        public Node GetById(int id)
        {
            var node = _context.Nodes.Find(id);

            if (node == null)
                throw new AppException("Node not found");

            return node;
        }
        public Node Create(Node node)
        {

            if (_context.Nodes.Any(x => x.Name == node.Name))
                throw new AppException("Node Name '" + node.Name + "' is already taken");

            if (!_context.Gateways.Any(x => x.Id == node.GatewayId))
                throw new AppException("Gateway Id '" + node.GatewayId + "' doesnt exist");


            _context.Nodes.Add(node);
            _context.SaveChanges();

            return node;
        }

        public void Update(Node gatewayParam)
        {
            //get currect node in db
            var node = _context.Nodes.Find(gatewayParam.Id);

            if (node == null)
                throw new AppException("Node not found");

            if (gatewayParam.Name != null)
            {
                if (gatewayParam.Name != node.Name)
                {
                    // gatewayname has changed so check if the new gatewayname is already taken
                    if (_context.Nodes.Any(x => x.Name == gatewayParam.Name))
                        throw new AppException("gatewayname " + gatewayParam.Name + " is already taken");
                }
                node.Name = gatewayParam.Name;
            }

            // Change gateway
            if (gatewayParam.GatewayId != node.GatewayId)
                node.GatewayId = gatewayParam.GatewayId;

            // update node properties / cant chane Role
            if (!string.IsNullOrWhiteSpace(gatewayParam.Description))
                node.Description = gatewayParam.Description;

            _context.Nodes.Update(node);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var node = _context.Nodes.Find(id);

            if (node == null)
                throw new AppException("Node not found");

            else
            {
                _context.Nodes.Remove(node);
                _context.SaveChanges();
            }
        }

    }
}