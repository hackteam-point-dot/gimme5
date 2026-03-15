const http = require('@jetbrains/youtrack-scripting-api/http');

exports.httpHandler = {
  endpoints: [
    {
      method: 'GET',
      path: 'debug',
      handle: function handle(ctx) {
        const requestParam = ctx.request.getParameter('test');
        ctx.response.json({test: requestParam});
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
    }
  ]
};
