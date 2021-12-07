
using ApiModels.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamServer.Models;
using TeamServer.Services;

namespace TeamServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListenersController : ControllerBase
    {
        private readonly IListenerService _listener;

        public ListenersController(IListenerService listener)
        {
            _listener = listener;
        }
        [HttpGet]

        public IActionResult GetListeners()
        {
            var listeners = _listener.GetListeners();
            return Ok(listeners);
        }

        [HttpGet("{name}")]
        public IActionResult GetListener(string name)
        {
            var listener = _listener.GetListener(name);
            if(listener is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(listener);
            }
        }

        [HttpPost]
        public IActionResult StartListener([FromBody] StartHttpListenerRequest request)
        {
            var listener = new HttpListener(request.Name, request.BindPort);
            listener.Start();

            _listener.AddListener(listener);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/{listener.Name}";

            return Created(path, listener);
        }

        [HttpDelete("{name}")]
        public IActionResult RemoveListener(string name)
        {
            var listener = _listener.GetListener(name);
            if (listener is null)
            {
                return NotFound();
            }
            else
            {
                _listener.RemoveListener(listener);
                listener.Stop();
                return Ok(listener);
            }
        }
    }
}
