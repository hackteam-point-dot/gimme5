const http = require('@jetbrains/youtrack-scripting-api/http');
const workflow = require('@jetbrains/youtrack-scripting-api/workflow');

exports.buildEventPayloud = (ctx, eventType) => {
  const issue = ctx.issue;
  const user = ctx.currentUser;
  const project = issue.project;

  const settings = gimmeSettings(project.key);
  
  return JSON.stringify({
      event: eventType,
      login: user.login,
      projectKey: project.key,
      projectName: project.name,
      children: !!issue.links['parent for'] ? issue.links['parent for'].map(x => x.id) : null,
      issueId: issue.id,
      storyPoints: issue[settings.issueWeightFieldName]?.toString(),
      dueDate: issue["Due Date"],
      issuePriority: issue.Priority.name
    });
};

exports.gimmeSettings = (projectId) => gimmeSettings(projectId);

function gimmeSettings(projectId) {
    
  const connection = new http.Connection('https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net');
  connection.addHeader({name: 'Content-Type', value: 'application/json'});
  const response = connection.getSync(`/api/ProjectConfiguration?projectId=${projectId}`, '');
    
  console.log(`response ${response}`);
  
  return JSON.parse(response.response);
}

exports.sendEvent = (payload) => {
  console.log(`Payload ${payload}`);
    
  const connection = new http.Connection('https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net');
  connection.addHeader({name: 'Content-Type', value: 'application/json'});
  const response = connection.doSync('POST', '/api/events', '', payload);

  console.log(response);
  
  const result = JSON.parse(response.response);
  
  if (result && result.expChange){
    workflow.message(`🎉 Earned XP for closing ticket +${result.expChange}! 👏`);
  }
  
  if (result && result.levelUpgradedTo){
    workflow.message(`🎉 Level up to ${result.levelUpgradedTo}!${!!result.heroClass? ' Now you have title ' + result.heroClass + ' 🙌' : ''}`);
  }
  
  if (result && result.achievement){
    workflow.message(`🎉 Achievement(s) owned: ${result.achievement}!`+ (result.achievementExp ? (` Earned extra XP ${result.achievementExp}!`) : ''));
  }
  
  return response;
};