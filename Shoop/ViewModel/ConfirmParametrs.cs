namespace Shoop.ViewModel
{
    public class ConfirmParametrs
    {
        public int ID { get; set; }

        public string ActoinName { get; set; } = null!;

        public string ActionForWhat { get; set; } = null!;

        public bool IsTradne { get; set; }
        public bool IsProductExistInTrandy { get; set; }
    }
}
