﻿using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc;

namespace NJsonApi.Serialization
{
    internal class JsonApiActionFilter : IActionFilter
    {
        public bool AllowMultiple { get { return false; } }
        private readonly JsonApiTransformer jsonApiTransformer;
        private readonly Configuration configuration;

        public JsonApiActionFilter(JsonApiTransformer jsonApiTransformer, Configuration configuration)
        {
            this.jsonApiTransformer = jsonApiTransformer;
            this.configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                return;
            }

            var responseResult = (ObjectResult)context.Result;

            var jsonApiContext = new Context(configuration, new Uri(context.HttpContext.Request.GetDisplayUrl()));
            responseResult.Value = jsonApiTransformer.Transform(responseResult.Value, jsonApiContext);
        }
    }
}