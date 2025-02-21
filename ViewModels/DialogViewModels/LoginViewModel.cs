using NLog;
using Trace.Common;
using Trace.Common.Extensions;
using Trace.Dto;
using Trace.Service.IService;

namespace Trace.ViewModels.DialogViewModels
{
    class LoginViewModel : BindableBase,IDialogAware
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public LoginViewModel(IUserService Service, IEventAggregator aggregator)
        {
            UserDto = new RegisterDto();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.Service = Service;
            this.aggregator = aggregator;
        }

        public string Title { get; set; } = "货车行驶轨迹监测系统";



        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
           
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        #region Login

        private int selectIndex;

        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }


        public DelegateCommand<string> ExecuteCommand { get; private set; }


        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string passWord;
        private readonly IUserService Service;
        private readonly IEventAggregator aggregator;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
                case "Register": Resgiter(); break;
                case "RegisterPage": SelectIndex = 1; break;
                case "Return": SelectIndex = 0; break;
            }
        }

        private RegisterDto userDto;

        public RegisterDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }

        public DialogCloseListener RequestClose { get; }

        async void Login()
        {
            if (string.IsNullOrWhiteSpace(UserName) ||
                string.IsNullOrWhiteSpace(PassWord))
            {
                return;
            }

            var loginResult = await Service.LoginAsync(new UserDto()
            {
                Username = UserName,
                Password = PassWord
            });

            if (loginResult != null && loginResult.Status)
            {
                AppSession.UserName=loginResult.Result.Username;
                RequestClose.Invoke(ButtonResult.OK);
                aggregator.SendMessage("登录成功！");
                _logger.Info($"{AppSession.UserName}登录了");
            }
            else
            {
                RequestClose.Invoke(new DialogResult(ButtonResult.No));
                //登录失败提示...
                aggregator.SendMessage(loginResult.Message, "Login");
            }
        }

        private async void Resgiter()
        {
            if (string.IsNullOrWhiteSpace(UserDto.Username) ||
                string.IsNullOrWhiteSpace(UserDto.FullName) ||
                string.IsNullOrWhiteSpace(UserDto.Password) ||
                string.IsNullOrWhiteSpace(UserDto.NewPassWord))
            {
                aggregator.SendMessage("请输入完整的注册信息！", "Login");
                return;
            }

            if (UserDto.Password != UserDto.NewPassWord)
            {
                aggregator.SendMessage("密码不一致,请重新输入！", "Login");
                return;
            }

            var resgiterResult = await Service.RegisterAsync(new RegisterDto()
            {
                Username = UserDto.Username,
                FullName = UserDto.FullName,
                Password = UserDto.Password
            });

            if (resgiterResult != null && resgiterResult.Status)
            {
                aggregator.SendMessage("注册成功", "Login");
                //注册成功,返回登录页页面
                SelectIndex = 0;
            }
            else
                aggregator.SendMessage(resgiterResult.Message, "Login");
        }

        void LoginOut()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.No));
        }

        #endregion
    }
}
