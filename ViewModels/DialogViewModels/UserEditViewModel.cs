using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Trace.Common;
using Trace.Dto;

namespace Trace.ViewModels.DialogViewModels
{
    public class UserEditViewModel:BindableBase, IDialogHostAware
    {
        #region 属性
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private DelegateCommand openFileCommand;

        public DelegateCommand OpenFileCommand
        {
            get { return openFileCommand; }
            set { openFileCommand = value; }
        }
        private BitmapImage _currentImageSource;

        public BitmapImage CurrentImageSource
        {
            get { return _currentImageSource; }
            set { _currentImageSource = value; }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand CancelCommand { get ; set ; }
        public DelegateCommand SaveCommand { get ; set; }

        private UserDto userDto;

        public UserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }

        private string selectedRole;

        public string SelectedRole
        {
            get { return selectedRole; }
            set { selectedRole = value; RaisePropertyChanged(); }
        }

        #endregion


        public UserEditViewModel()
        {
            OpenFileCommand = new DelegateCommand(openFile);
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Save);
        }
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(UserDto.FullName) ||
               string.IsNullOrWhiteSpace(UserDto.Username)) return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时,把编辑的实体返回并且返回OK
                DialogParameters param = new DialogParameters();
                param.Add("Value", UserDto);
                var result = new DialogResult(ButtonResult.OK);
                result.Parameters = param;
                DialogHost.Close(DialogHostName, result);
            }
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No)); //取消返回NO告诉操作结束
        }
        private void openFile()
        {
            // 创建“打开文件”对话框
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // 设置文件类型过滤
            dlg.Filter = "图片|*.jpg;*.png;*.gif;*.bmp;*.jpeg";

            // 调用ShowDialog方法显示“打开文件”对话框
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // 获取所选文件名并在FileNameTextBox中显示完整路径
                string filename = dlg.FileName;
                FileName= filename;

                // 在image1中预览所选图片
                CurrentImageSource = new BitmapImage(new Uri(filename));
                
               
            }
        }
        public void OnDialogOpen(IDialogParameters dialogParameters)
        {
            if (dialogParameters.ContainsKey("Value"))
            {
                UserDto = dialogParameters.GetValue<UserDto>("Value");
            }
            else
                UserDto = new UserDto();
        }
        private void UploadIMG()
        {
            // 将所选文件读入字节数组
            byte[] img = File.ReadAllBytes(FileName);
            string fileName = Path.GetFileNameWithoutExtension(FileName);

            // 创建数据库连接
            string connectionString = "your_connection_string_here";
            string sql = "INSERT INTO Users (Username, Avatar) VALUES (@Username, @Avatar)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", fileName);
                    cmd.Parameters.AddWithValue("@Avatar", img);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

       
    }
}
