namespace TaskManagerApi.Services.Abstracts
{
    public abstract class AbstractService
    {
        protected bool DoAction(Action action)
        {
            if (action == null) return false;

            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
