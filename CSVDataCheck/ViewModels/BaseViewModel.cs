using System;
using System.ComponentModel;

namespace CSVDataCheck.ViewModels
{
    /// <summary>
    /// 绑定数据属性
    /// 实现了INotifyPropertyChanged接口,其属性成员才具备通知UI的能力
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
