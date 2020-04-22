using bcfamilyalbum_api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Extensions
{
    public class LockableTreeItemPhysicalFileResult : PhysicalFileResult, IActionResult
    {
        TreeItem _undelyingItem;

        public LockableTreeItemPhysicalFileResult(TreeItem item, MediaTypeHeaderValue contentType) : base(item.FullPath, contentType)
        {
            _undelyingItem = item;
        }

        public LockableTreeItemPhysicalFileResult(TreeItem item, string contentType) : base(item.FullPath, contentType)
        {
            _undelyingItem = item;
        }

        public override void ExecuteResult(ActionContext context)
        {
            throw new Exception("Lets assume for now that this should be called asynchronically");
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            try
            {
                await _undelyingItem.Lock();
                await base.ExecuteResultAsync(context);
            } finally
            {
                _undelyingItem.Release();
            }
        }
    }
}
