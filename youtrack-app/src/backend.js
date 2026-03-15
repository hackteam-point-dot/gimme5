const http = require('@jetbrains/youtrack-scripting-api/http');

exports.httpHandler = {
  endpoints: [
    {
      method: 'GET',
      path: 'leaderboard',
      handle: function handle(ctx) {
        const limit = ctx.request.getParameter('limit');
        const skip = ctx.request.getParameter('skip');
        const connection = new http.Connection('https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net');
        connection.addHeader({name: 'Content-Type', value: 'application/json'});
        const response = connection.getSync('/api/Dashboard/leaderboard?limit=' + encodeURIComponent(limit) + '&skip=' + encodeURIComponent(skip), '');
        ctx.response.json(JSON.parse(response.response));
      }
    },
    {
      method: 'GET',
      path: 'user-card',
      handle: function handle(ctx) {
        const userId = ctx.request.getParameter('userId');
        const connection = new http.Connection('https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net');
        connection.addHeader({name: 'Content-Type', value: 'application/json'});
        const response = connection.getSync('/api/UserProfile/card?userId=' + encodeURIComponent(userId), '');
        ctx.response.json(JSON.parse(response.response));
      }
    },
    {
      method: 'GET',
      path: 'project-configuration',
      handle: function handle(ctx) {
        const projectId = ctx.request.getParameter('projectId');
        const connection = new http.Connection('https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net');
        connection.addHeader({name: 'Content-Type', value: 'application/json'});
        const response = connection.getSync('/api/ProjectConfiguration?projectId=' + encodeURIComponent(projectId), '');
        ctx.response.json(JSON.parse(response.response));
      }
    },
    {
      method: 'PUT',
      path: 'project-configuration',
      handle: function handle(ctx) {
        var body = ctx.request.body;
        var connection = new http.Connection('https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net');
        connection.addHeader({name: 'Content-Type', value: 'application/json'});
        var response = connection.putSync('/api/ProjectConfiguration', [], body);
        ctx.response.json(JSON.parse(response.response));
      }
    },
    {
      method: 'POST',
      path: 'easter-egg',
      handle: function handle(ctx) {
        var body = ctx.request.body;
        var connection = new http.Connection('https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net');
        connection.addHeader({name: 'Content-Type', value: 'application/json'});
        const response = connection.doSync('POST', '/api/events/flappy-bird', '', body);
        ctx.response.json(JSON.parse(response.response));
      }
    }
  ]
};
