using SqlSugar;

namespace Surging.DBModel.Models
{
    public partial class User
    {
        [SugarColumn(IsPrimaryKey = true)]//标识是否为主键
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
    }
}
