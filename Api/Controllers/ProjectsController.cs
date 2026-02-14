using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers{

[ApiController]
[Route("api/projects")]

public class ProjectsController : ControllerBase
{

    private static readonly List<Project> Projects = new();

    [Authorize]
    [HttpGet]
    public IActionResult GetProjects(){
        return Ok(
            Projects
        );
    }

    // Get Project by Id
    [HttpGet("{Id}")]
    public IActionResult GetProjectById(int Id){
        var project = Projects.FirstOrDefault(p => p.Id == Id);

        if(project != null){
            return Ok(project);
        }else{
            return NotFound();
        }
    }


    // POST /api/projects
    [HttpPost]
    public IActionResult CreateProject(CreateProjectRequest request){

        var project = new Project{
            Id = Projects.Count + 1,
            Name = request.Name
        };

        Projects.Add(project);

        return CreatedAtAction(
            nameof(GetProjects),
            new {id = project.Id},
            project
        );

    }

    // Put 
    [HttpPut("{id}")]
    public IActionResult UpdateProject(int id,UpdateProjectRequest UpdatedProject){
        var project = Projects.FirstOrDefault(p => p.Id == id);

        if(project!=null){
         project.Name =   UpdatedProject.Name; 
         return CreatedAtAction(
            nameof(GetProjects),
            new {id = project.Id},
            project
         );  
        }else{
            return NotFound();
        }
    }

    // delete
    [HttpDelete("id")]
    public IActionResult DeleteProject(int id){
        var project = Projects.FirstOrDefault(p => p.Id == id);

        if(project != null){
            Projects.Remove(project);
            return NoContent();
        }

        return NotFound();
    }
}

// MODEL
public class Project{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

// DTO 
public class CreateProjectRequest{
    public string Name { get; set; } = string.Empty; 
}
 
// DTO 
public class UpdateProjectRequest{
    public string Name { get; set; } = string.Empty; 
}

}