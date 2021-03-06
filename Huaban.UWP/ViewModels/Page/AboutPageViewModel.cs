﻿using System.Collections.ObjectModel;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	public class AboutPageViewModel : HBViewModel
	{
		public AboutPageViewModel(Context context)
			: base(context)
		{
			Title = "关于";
		}
		public ObservableCollection<NavItemModel> AboutList { set; get; } = new ObservableCollection<NavItemModel>(new NavItemModel[] {
			new NavItemModel() { Title="应用名称",Label="爱花瓣UWP" },
			new NavItemModel() { Title="声明",Label="第三方App，非官方"},
			new NavItemModel() { Title="博客园",Label="http://www.cnblogs.com/dblleaf"},
			new NavItemModel() { Title="作者",Label="song-zhaoli"},
			new NavItemModel() { Title="Email",Label="song-zhaoli@outlook.com"},
			new NavItemModel() { Title="微博",Label="@宋小召召"},
			new NavItemModel() { Title="版本",Label="v1.1.2"},
			new NavItemModel() { Label="花瓣，无论你是什么样的人群，都能找到自己喜欢的美图。当然还有些，你懂得，现在就开始去发现吧！"}
		});
	}
}
