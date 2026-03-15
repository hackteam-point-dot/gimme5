const entities = require('@jetbrains/youtrack-scripting-api/entities');
const utils = require('./utils');

exports.rule = entities.Issue.onChange({
  title: 'Issue_in_progress',
  guard: (ctx) => {
    const issue = ctx.issue;
    return issue.fields.Type.name === 'Task' && issue.fields.State.name === "In Progress";
  },
  action: (ctx) => {
    const payload = utils.buildEventPayloud(ctx, 'ISSUE_IN_PROGRESS');
    
    utils.sendEvent(payload);
  },
  requirements: {
    // TODO: add requirements
  }
});