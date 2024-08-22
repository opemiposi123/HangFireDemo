using Hangfire;
using HangFireDemo.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangFireDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		[HttpPost]
		[Route("CreateBackgroundJob")]
		public ActionResult CreateBackgroundJob()
		{
			BackgroundJob.Enqueue<TestJob>(x => x.WriteLog("Background Job Triggered"));
			return Ok();
		}

		[HttpPost]
		[Route("CreateScheduledJob")]
		public ActionResult CreateScheduledJob()
		{
			var scheduleDatetime = DateTime.UtcNow.AddSeconds(5);
			var dateTimeOffset = new DateTimeOffset(scheduleDatetime);
			//BackgroundJob.Schedule(() => Console.WriteLine("Scheduled Job Triggered"), dateTimeOffset);
			BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Scheduled Job Triggered"), dateTimeOffset);
			return Ok();
		}

		[HttpPost]
		[Route("CreateContiniousJob")]
		public ActionResult CreateContiniousJob() 
		{
			var scheduleDatetime = DateTime.UtcNow.AddSeconds(5);
			var dateTimeOffset = new DateTimeOffset(scheduleDatetime);
			var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Scheduled Job 2 Triggered"), dateTimeOffset);

			var job2Id = BackgroundJob.ContinueJobWith<TestJob>(jobId, x => x.WriteLog("Continious Job 1 Triggered"));
			var job3Id = BackgroundJob.ContinueJobWith<TestJob>(job2Id, x => x.WriteLog("Continious Job 2 Triggered"));
			var job4Id = BackgroundJob.ContinueJobWith<TestJob>(job3Id, x => x.WriteLog("Continious Job 3 Triggered"));
			return Ok();
		}

		[HttpPost]
		[Route("CreateReccuringJob")]
		public ActionResult CreateReccuringJob()
		{
			RecurringJob.AddOrUpdate<TestJob>("RecurringJob1", x => x.WriteLog("Recurring Job Triggered"), "*/1 * * * *");
			return Ok();
		}
	}
}
