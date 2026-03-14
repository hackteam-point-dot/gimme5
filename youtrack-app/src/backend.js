const http = require('http');

exports.httpHandler = {
  endpoints: [
    {
      method: 'GET',
      path: 'debug',
      handle: function handle(ctx) {
        // See https://www.jetbrains.com/help/youtrack/devportal-apps/apps-reference-http-handlers.html#request
        const requestParam = ctx.request.getParameter('test');
        // See https://www.jetbrains.com/help/youtrack/devportal-apps/apps-reference-http-handlers.html#response
        ctx.response.json({test: requestParam});
      }
    },
    {
      method: 'GET',
      path: 'users/{userId}/card',
      handle: function handle(ctx) {
        const userId = ctx.pathParameters.userId;
        if (!userId) {
          ctx.response.status(400);
          ctx.response.json({error: 'Missing userId'});
          return;
        }

        const baseUrl = ctx.settings.backendBaseUrl;
        if (!baseUrl) {
          ctx.response.status(500);
          ctx.response.json({error: 'Backend base URL not configured'});
          return;
        }

        const connection = http.getConnection(baseUrl);
        const response = connection.getSync('/api/UserProfile/card?userId=' + encodeURIComponent(userId), {});

        if (response && response.code === 200) {
          ctx.response.json(JSON.parse(response.response));
        } else {
          ctx.response.status(response ? response.code : 502);
          ctx.response.json({error: 'Failed to fetch user card data'});
        }
      }
    }
  ]
};
