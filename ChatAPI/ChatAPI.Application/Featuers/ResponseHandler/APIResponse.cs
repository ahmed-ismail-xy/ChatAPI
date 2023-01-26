namespace ChatAPI.Application.Featuers.ResponseHandler
{
    public class APIResponse
    {
        public bool Success => Errors == null;
        public string Message { get; set; }
        public List<int> Errors { get; private set; }

        public void AddError(int error)
        {
            if (Errors == null)
                Errors = new List<int>();

            Errors.Add(error);
        }
        public void AddErrors(List<int> errors)
        {
            if (Errors == null)
                Errors = new List<int>();

            Errors.AddRange(errors);
        }
    }
    public class APIResponse<T> : APIResponse
    {
        public T? Data { get; set; }
    }
}
