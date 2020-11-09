using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using QRSpace.Client.Models.Shogi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QRSpace.Client.Pages
{
    public class ShogiBase : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private static HubConnection _hubConnection;

        public bool IsConnected =>
            _hubConnection.State == HubConnectionState.Connected;

        private string _opponentName;
        
        protected static readonly GameBoard Board = new GameBoard();
        private bool _isControlling = true;
        private (int x, int y) _selectedPos = (9, 9);
        private List<(int x, int y)> _legalMoves = new List<(int x, int y)>();

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/shogihub"))
                .Build();
            
            _hubConnection.On<(int, int), (int, int)>("ReceiveStep", (from, to) =>
            {
                var (item1, item2) = from;
                var _from = (8 - item1, 8 - item2);
                var (item3, item4) = to;
                var _to = (8 - item3, 8 - item4);
                Board.MoveTo(_from, _to);
                ChangeSide();
            });

            _hubConnection.On("GameConfirmed", (bool isFirst) =>
            {
                _isControlling = isFirst;
            });

            _hubConnection.On("ReceiveInvitation", (string inviterUserName) =>
            {
                //if
                //Confirm()
                _hubConnection.SendAsync("GetConfirmation", inviterUserName);
                _opponentName = inviterUserName;
                //else
                
            });

            _hubConnection.On("GameOverConfirmed", () =>
            {
                
            });

            await _hubConnection.StartAsync();

            await _hubConnection.SendAsync("UserLogin", $"User{DateTime.Now.Ticks.ToString()}");
        }

        // TODO : Contact List
        protected async Task GameStart(string userName)
        {
            _opponentName = userName;
            _isControlling = DateTime.Now.Ticks % 2 == 0;
            await _hubConnection.SendAsync("GameStartCheck", userName);
        }

        protected Task SendStep((int, int) from, (int, int) to)
        {
            return _hubConnection.SendAsync("SendStep", from, to);
        }

        public void Dispose()
        {
            _ = _hubConnection.DisposeAsync();
        }

        //TODO: OnClick
        protected void OnCellClick(MouseEventArgs args, (int x, int y) pos)
        {
            if (Board.Cells[pos.x, pos.y].Piece != null && !Board.Cells[pos.x, pos.y].Piece.Player)
            {
                _selectedPos = pos;
                _legalMoves = Board.Cells[pos.x, pos.y].Piece.LegalMoves;
                StateHasChanged();
            }
            else if (_selectedPos != (9, 9))//Selected state
            {
                if (!_legalMoves.Contains(pos)) return;
                Board.MoveTo(_selectedPos, pos);
                SendStep(_selectedPos, pos);
                _selectedPos = (9, 9);
                _legalMoves.Clear();
                StateHasChanged();
                ChangeSide();
            }
        }

        protected void ChangeSide()
        {
            //TODO: ChangeSide
            _isControlling = !_isControlling;
        }

        protected string SelfCellStyle(int x, int y, int state)
        {
            var result = "cell-no-piece";
            switch (state)
            {
                case 0:
                    result = "cell";
                    if (_selectedPos == (x, y))
                        result = "cell-selected";
                    break;

                default:
                    if (_legalMoves.Contains((x, y)))
                        result = "cell-movable";
                    break;
            }
            return result;
        }
    }
}