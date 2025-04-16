namespace School.PL.Helper.Services
{
    public class MyRecurringJob
    {
        private readonly IClassesServices _classesServices;

        public MyRecurringJob(IClassesServices classesServices)
        {
            _classesServices = classesServices;
        }

        public async Task RunJobAsync()
        {
            var classes = await _classesServices.GetAllClassAsync();
            Console.WriteLine($"Job executed. Retrieved {classes.Count()} classes.");
        }
    }

}
