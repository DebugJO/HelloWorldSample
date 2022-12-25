using Caliburn.Micro;
using MvvmExample.Helpers;
using MvvmExample.Models;
using MvvmExample.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MvvmExample.ViewModels;

public class FormFirstViewModel : Screen
{
    private readonly IUserInfoService mUserInfoService;
    private ObservableCollection<UserInfo> mUserInfoListModel;
    private UserInfo? mUserInfoSelectedModel;

    private int mSelectedIndex;
    private string mID;
    private string mName;

    public FormFirstViewModel(IUserInfoService userInfoService)
    {
        mUserInfoService = userInfoService;
        mUserInfoListModel = new ObservableCollection<UserInfo>();
        mUserInfoSelectedModel = new();

        mSelectedIndex = -1;
        mID = string.Empty;
        mName = string.Empty;
    }

    public ObservableCollection<UserInfo> UserInfoListModel
    {
        get => mUserInfoListModel;
        set
        {
            mUserInfoListModel = value;
            NotifyOfPropertyChange();
        }
    }

    public UserInfo? UserInfoSelectedModel
    {
        get => mUserInfoSelectedModel;
        set
        {
            mUserInfoSelectedModel = value;
            NotifyOfPropertyChange();
        }
    }

    public int SelectedIndex
    {
        get => mSelectedIndex;
        set
        {
            mSelectedIndex = value;
            NotifyOfPropertyChange();
        }
    }

    public string ID
    {
        get => mID;
        set
        {
            mID = value;
            NotifyOfPropertyChange();
        }
    }

    public string Name
    {
        get => mName;
        set
        {
            mName = value;
            NotifyOfPropertyChange();
        }
    }

    public async void ButtonSearch(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn)
        {
            return;
        }

        try
        {
            btn.Content = "조회 중...";
            btn.IsEnabled = false;
            ClearUserInfo();

            UserInfo user = new() { ID = "", Name = "" };

            List<UserInfo> userList = await mUserInfoService.GetUserInfoList(user); // async await , 프로퍼티는 Thread안전.. 직접 View컨트롤 Dispatcher.Invoke

            UserInfoListModel.Clear();
            UserInfoListModel = new(userList);

            WindowHelper.InfoMessage("조회완료", "조회결과 :" + userList.Count.ToString());
            LogHelper.Logger.Error("조회완료 : 조회결과 : " + userList.Count.ToString());
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            WindowHelper.InfoMessage("xxx조회오류", message);
            LogHelper.Logger.Error("xxx조회오류 : " + message);
        }
        finally
        {
            btn.Content = "조회";
            btn.IsEnabled = true;
        }
    }

    public void ListViewSelectUserInfo(object sender, MouseButtonEventArgs e)
    {
        try
        {
            //ClearUserInfo();

            if (SelectedIndex >= 0)
            {
                UserInfo user = UserInfoListModel[SelectedIndex];
                ID = user.ID;
                Name = user.Name;
            }

            /* 위와 동일한 결과 : 필요에 따라 바꾸어가면서 사용
            if (sender is not ListView listView)
            {
                return;
            }

            if (UserInfoSelectedModel != null && listView.Items.Count > 0)
            {
                ID = UserInfoSelectedModel.ID;
                Name = UserInfoSelectedModel.Name;
            }
            */
        }
        catch (Exception ex)
        {
            WindowHelper.InfoMessage("선택 오류", ex.Message);
            LogHelper.Logger.Error("선택 오류 : " + ex.Message);
        }
    }

    private void ClearUserInfo()
    {
        ID = string.Empty;
        Name = string.Empty;
    }
}
