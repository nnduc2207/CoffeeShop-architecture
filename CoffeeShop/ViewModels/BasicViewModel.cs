using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace CoffeeShop.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class RelayCommand<T> : ICommand
        {
            private readonly Predicate<T> _canExecute;
            private readonly Action<T> _execute;

            public RelayCommand(Predicate<T> canExecute, Action<T> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }
                _canExecute = canExecute;
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                try
                {
                    return _canExecute == null ? true : _canExecute((T)parameter);
                }
                catch
                {
                    return true;
                }
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
        public AsyncObservableCollection<dynamic> SearchByName(string inputKeyWord, AsyncObservableCollection<dynamic> inputList)
        {
            AsyncObservableCollection<dynamic> outputList = new AsyncObservableCollection<dynamic>();
            foreach (var item in inputList)
            {
                outputList.Add(item);
            }
            if (inputKeyWord != "" && inputKeyWord != null)
            {
                List<string> listWordsAND = new List<string>();
                List<string> listWordsOR = new List<string>();
                List<string> listWordsNOT = new List<string>();

                List<string> listWords = new List<string>();
                List<string> listOperators = new List<string>();
                string[] words = inputKeyWord.Trim().ToLower().Split(' ');
                string wordElement = "";
                foreach (string word in words)
                {
                    if (word == "and" || word == "or" || word == "not")
                    {
                        listWords.Add(wordElement);
                        wordElement = "";
                        if (listOperators.Count > 0)
                        {
                            foreach (string item in listWords)
                            {
                                if (listOperators[0] == "and")
                                {
                                    listWordsAND.Add(item);
                                }
                                else if (listOperators[0] == "or")
                                {
                                    listWordsOR.Add(item);
                                }
                                else if (listOperators[0] == "not")
                                {
                                    listWordsNOT.Add(item);
                                }
                            }
                            listWords.RemoveRange(0, listWords.Count);
                            string temp = listOperators[0];
                            listOperators.RemoveAt(0);
                            listOperators.Add(word);
                            listOperators.Add(temp);
                        }
                        else if (word == "not" && listWords.Count > 0)
                        {
                            listWordsOR.Add(listWords[0]);
                            listOperators.Add(word);
                            listWords.RemoveRange(0, listWords.Count);
                        }
                        else
                        {
                            listOperators.Add(word);
                        }
                    }
                    else
                    {
                        if (wordElement == "")
                        {
                            wordElement += word;
                        }
                        else
                        {
                            wordElement += " " + word;
                        }
                    }
                }
                if (wordElement != "")
                {
                    listWords.Add(wordElement);
                    wordElement = "";
                }
                if (listOperators.Count > 0)
                {
                    foreach (string item in listWords)
                    {
                        if (listOperators[0] == "and")
                        {
                            listWordsAND.Add(item);
                        }
                        else if (listOperators[0] == "or")
                        {
                            listWordsOR.Add(item);
                        }
                        else if (listOperators[0] == "not")
                        {
                            listWordsNOT.Add(item);
                        }
                    }
                    listWords.RemoveRange(0, listWords.Count);
                    listOperators.RemoveAt(0);
                }
                else if (listWords.Count > 0)
                {
                    foreach (string item in listWords)
                    {
                        listWordsOR.Add(item);
                    }
                }


                DataTable rawTable = new DataTable();
                rawTable.Columns.Add("Name", typeof(string));
                rawTable.Columns.Add("Order", typeof(int));
                rawTable.Columns.Add("Distance", typeof(int));
                rawTable.Columns.Add("Length", typeof(int));

                DataTable resultTable = new DataTable();
                resultTable = rawTable.Copy();

                foreach (dynamic item in inputList)
                {
                    rawTable.Rows.Add(item.Ten, 0, 0, 0);
                }

                DataTable notTable = new DataTable();
                if (listWordsNOT.Count > 0)
                {
                    notTable = rawTable.Copy();
                    foreach (string word in listWordsNOT)
                    {
                        notTable = SearchOneWord(word, notTable);
                    }
                }


                DataTable andTable = new DataTable();
                if (listWordsAND.Count > 0)
                {
                    andTable = rawTable.Copy();
                    foreach (string word in listWordsAND)
                    {
                        andTable = SearchOneWord(word, andTable);
                    }
                    foreach (DataRow rowData in andTable.Rows)
                    {
                        resultTable.Rows.Add(rowData.ItemArray);
                    }
                }


                foreach (string word in listWordsOR)
                {
                    DataTable orTable = SearchOneWord(word, rawTable);
                    foreach (DataRow rowDataOrTable in orTable.Rows)
                    {
                        bool flag = true;
                        foreach (DataRow rowDataResultTable in resultTable.Rows)
                        {
                            if (rowDataOrTable["Name"] == rowDataResultTable["Name"])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag == true)
                        {
                            resultTable.Rows.Add(rowDataOrTable.ItemArray);
                        }
                    }
                }
                for (int x = 0; x < notTable.Rows.Count; x++)
                {
                    for (int y = 0; y < resultTable.Rows.Count; y++)
                    {
                        if (notTable.Rows[x].Field<string>(0) == resultTable.Rows[y].Field<string>(0))
                        {
                            resultTable.Rows.RemoveAt(y);
                        }
                    }
                }

                int tableRange = resultTable.Rows.Count;
                int listRange = inputList.Count;

                DataView DV = resultTable.DefaultView;
                DV.Sort = "Order DESC, Distance ASC, Length ASC";
                resultTable = DV.ToTable();


                outputList.Clear();

                for (int tableIndex = 0; tableIndex < tableRange; tableIndex++)
                {

                    for (int listIndex = 0; listIndex < listRange; listIndex++)
                    {
                        if (resultTable.Rows[tableIndex].Field<string>(0) == inputList[listIndex].Ten)
                        {
                            outputList.Add(inputList[listIndex]);
                            break;
                        }
                    }
                }
            }
            return outputList;
        }

        public DataTable SearchOneWord(string word, DataTable inputTable)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("Name", typeof(string));
            resultTable.Columns.Add("Order", typeof(int));
            resultTable.Columns.Add("Distance", typeof(int));
            resultTable.Columns.Add("Length", typeof(int));

            int tableRange = inputTable.Rows.Count;
            for (int tableIndex = 0; tableIndex < tableRange; tableIndex++)
            {
                string recipeName = inputTable.Rows[tableIndex].Field<string>(0);

                int order = -1;
                int Distance = 0;
                int prevWordIndex = 0;
                int signedWordIndex = recipeName.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                int unsignedWordIndex = RemoveSign(recipeName).IndexOf(RemoveSign(word), StringComparison.OrdinalIgnoreCase);


                if (signedWordIndex >= 0)
                {
                    if (IsSeparateWord(word, recipeName, signedWordIndex) == true)
                    {
                        order = word.Length + 2;
                    }
                    else if (IsBeginOfWord(word, recipeName, signedWordIndex) == true)
                    {
                        order = word.Length - 1;
                    }
                    else
                    {
                        continue;
                    }

                    if (signedWordIndex >= prevWordIndex)
                    {
                        Distance += signedWordIndex - prevWordIndex;
                    }
                    else
                    {
                        Distance += recipeName.Length;
                    }
                    prevWordIndex = signedWordIndex;
                }
                else if (unsignedWordIndex >= 0)
                {
                    if (IsSeparateWord(word, recipeName, unsignedWordIndex) == true)
                    {
                        order = word.Length + 1;
                    }
                    else if (IsBeginOfWord(word, recipeName, unsignedWordIndex) == true)
                    {
                        order = word.Length - 2;
                    }
                    else
                    {
                        continue;
                    }
                    if (unsignedWordIndex >= prevWordIndex)
                    {
                        Distance += unsignedWordIndex - prevWordIndex;
                    }
                    else
                    {
                        Distance += recipeName.Length;
                    }
                    prevWordIndex = unsignedWordIndex;
                }

                if (order > -1)
                {
                    resultTable.Rows.Add(recipeName, order, Distance, recipeName.Length);
                }

            }
            return resultTable;
        }
        public static string RemoveSign(string inputString)
        {
            string[] VietNamChar = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    inputString = inputString.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return inputString;
        }

        private static bool IsSeparateWord(string word, string sentence, int index)
        {
            bool result = true;
            if (index > 0 && sentence[index - 1] != ' ')
            {
                result = false;
            }
            if (index + word.Length < sentence.Length && sentence[index + word.Length] != ' ')
            {
                result = false;
            }
            return result;
        }

        public static bool IsBeginOfWord(string word, string sentence, int index)
        {
            bool result = true;
            if (index > 0 && sentence[index - 1] != ' ')
            {
                result = false;
            }
            return result;
        }
    }

    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        private static object _syncLock = new object();

        public AsyncObservableCollection()
        {
            enableCollectionSynchronization(this, _syncLock);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                var eh = CollectionChanged;
                if (eh == null) return;

                var dispatcher = (from NotifyCollectionChangedEventHandler nh in eh.GetInvocationList()
                                  let dpo = nh.Target as DispatcherObject
                                  where dpo != null
                                  select dpo.Dispatcher).FirstOrDefault();

                if (dispatcher != null && dispatcher.CheckAccess() == false)
                {
                    dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => OnCollectionChanged(e)));
                }
                else
                {
                    foreach (NotifyCollectionChangedEventHandler nh in eh.GetInvocationList())
                        nh.Invoke(this, e);
                }
            }
        }

        private static void enableCollectionSynchronization(IEnumerable collection, object lockObject)
        {
            var method = typeof(BindingOperations).GetMethod("EnableCollectionSynchronization",
                                    new Type[] { typeof(IEnumerable), typeof(object) });
            if (method != null)
            {
                // It's .NET 4.5
                method.Invoke(null, new object[] { collection, lockObject });
            }
        }
    }
}
