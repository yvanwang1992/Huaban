﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Controls;
	using Models;
	using Windows.Foundation;

	public class UserPageViewModel : HBViewModel
	{
		public UserPageViewModel(Context context)
			: base(context)
		{
			MyPinListViewModel = new PinListViewModel(context, GetPinList);
			LikePinListViewModel = new PinListViewModel(context, GetLikePinList);
			BoardListViewModel = new BoardListViewModel(context, GetBoardList);
			FollowingListViewModel = new UserListViewModel(context, GetFollowingUserList);
			FollowerListViewModel = new UserListViewModel(context, GetFollowerList);
			//BoardListViewModel.BoardList = context.BoardList;
		}

		#region Properties

		private User _User;
		public User User
		{
			get { return _User; }
			set { SetValue(ref _User, value); }
		}

		public PinListViewModel MyPinListViewModel { private set; get; }

		public PinListViewModel LikePinListViewModel { private set; get; }

		public BoardListViewModel BoardListViewModel { private set; get; }

		public UserListViewModel FollowingListViewModel { private set; get; }

		public UserListViewModel FollowerListViewModel { private set; get; }

		#endregion

		#region Commands


		#endregion

		#region Methods

		private async Task<IEnumerable<Pin>> GetPinList(uint startIndex, int page)
		{
			IsLoading = true;

			List<Pin> list = new List<Pin>();
			try
			{
				list = await Context.API.UserAPI.GetPins(User?.user_id, MyPinListViewModel.GetMaxPinID());
				foreach (var item in list)
				{
					item.Width = MyPinListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((MyPinListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
				}
				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		private async Task<IEnumerable<Pin>> GetLikePinList(uint startIndex, int page)
		{
			IsLoading = true;

			List<Pin> list = new List<Pin>();
			try
			{
				list = await Context.API.UserAPI.GetLikePins(User?.user_id, LikePinListViewModel.GetMaxSeq());
				foreach (var item in list)
				{
					item.Width = LikePinListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((LikePinListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
				}
				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		private async Task<IEnumerable<Board>> GetBoardList(uint startIndex, int page)
		{
			BoardListViewModel.BoardList.NoMore();
			IsLoading = true;

			List<Board> list = new List<Board>();

			try
			{
				await Task.Delay(300);
				list = await Context.API.UserAPI.GetBoards(User?.user_id, BoardListViewModel.GetMaxSeq());

				if (list.Count == 0)
					BoardListViewModel.BoardList.NoMore();
				else
					BoardListViewModel.BoardList.HasMore();

				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		public async Task<IEnumerable<User>> GetFollowerList(uint startIndex, int page)
		{
			FollowerListViewModel.UserList.NoMore();
			IsLoading = true;
			var list = new List<User>();
			try
			{
				await Task.Delay(300);
				list = await Context.API.UserAPI.GetFollowerList(User?.user_id, FollowerListViewModel.GetMaxSeq());

				if (list.Count == 0)
					FollowerListViewModel.UserList.NoMore();
				else
					FollowerListViewModel.UserList.HasMore();

				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		public async Task<IEnumerable<User>> GetFollowingUserList(uint startIndex, int page)
		{

			FollowingListViewModel.UserList.NoMore();
			IsLoading = true;
			var list = new List<User>();
			try
			{
				await Task.Delay(300);
				list = await Context.API.UserAPI.GetFollowingUserList(User?.user_id, FollowingListViewModel.GetMaxSeq());

				if (list.Count == 0)
					FollowingListViewModel.UserList.NoMore();
				else
					FollowingListViewModel.UserList.HasMore();

				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			try
			{
				var user = e.Parameter as User;
				if (user == null || user == User)
					return;

				if (user.user_id == Context.User?.user_id)
				{
					User = user;
					Context.BoardListVM.BoardList = BoardListViewModel.BoardList;
				}

				else
					User = await Context.API.UserAPI.GetUser(user.user_id);

				await MyPinListViewModel.ClearAndReload();
				await LikePinListViewModel.ClearAndReload();
				await BoardListViewModel.ClearAndReload();
			}
			catch { }
		}

		public override Size ArrangeOverride(Size finalSize)
		{
			MyPinListViewModel.SetWidth(finalSize.Width);
			LikePinListViewModel.SetWidth(finalSize.Width);
			return base.ArrangeOverride(finalSize);
		}
		#endregion
	}
}
