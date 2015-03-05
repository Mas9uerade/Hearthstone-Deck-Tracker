﻿#region

using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.HearthStats.API;
using Hearthstone_Deck_Tracker.Hearthstone;

#endregion

namespace Hearthstone_Deck_Tracker
{
	/// <summary>
	/// Interaction logic for DeckNotes.xaml
	/// </summary>
	public partial class DeckNotes
	{
		private Deck _currentDeck;
		private bool _noteChanged;

		public DeckNotes()
		{
			InitializeComponent();
		}

		public void SetDeck(Deck deck)
		{
			_currentDeck = deck;
			Textbox.Text = deck.Note;
			_noteChanged = false;
			BtnSave.IsEnabled = false;
		}

		private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_currentDeck.Note = Textbox.Text;
			_noteChanged = true;
			BtnSave.IsEnabled = true;
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			SaveDeck();
		}

		public void SaveDeck()
		{
			if(!_noteChanged)
				return;
			DeckList.Save();
			if(Config.Instance.HearthStatsAutoUploadNewDecks)
			{
				Logger.WriteLine(string.Format("auto updating {0} deck", _currentDeck), "NoteDialog");
				HearthStatsManager.UpdateDeckAsync(_currentDeck, background: true);
			}
			_noteChanged = false;
			BtnSave.IsEnabled = false;
			Helper.MainWindow.DeckPickerList.UpdateDecks();
		}
	}
}