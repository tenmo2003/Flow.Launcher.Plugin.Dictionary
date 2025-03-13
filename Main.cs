using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.FreeDictionary
{
    public class Dictionary : IAsyncPlugin
    {
        private PluginInitContext _context;
        private QueryService _queryService;

        public async Task InitAsync(PluginInitContext context)
        {
            _context = context;
            _queryService = new QueryService();
        }

        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            if (string.IsNullOrEmpty(query.Search))
            {
                return new List<Result> { new() { Title = "Enter the word you wish to conquer! ╰( ◕ ᗜ ◕ )╯", IcoPath = "icon.png" } };
            }

            if (token.IsCancellationRequested) {
                await File.AppendText("flow-log.txt").WriteLineAsync($"Query: {query.Search} Cancelled");
                return null;
            }

            return await _queryService.Query(query.Search);
        }
    }
}