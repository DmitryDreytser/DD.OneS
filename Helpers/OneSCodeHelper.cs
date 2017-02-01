// Decompiled with JetBrains decompiler
// Type: DD.OneS.Helpers.OneSCodeHelper
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DD.OneS.Helpers
{
  internal class OneSCodeHelper
  {
    private const string DELIMITERS = "(\\.|\\,|\\;|\\\\|\\(|\\)|\\=|\\+|\\-|\\*|\\/{1}|\\?|<|>|\\[|\\])";
    private const string TEXT_STRING = "[\"|\\|](\\.|[^\"]|(\"\"))*[\"|\\r\\n]";
    private const string TEXT_BLOCK = "[\"|\\|](\\.|[^\"]|(\"\"))*";
    private const string DATE_STRING = "('\\d\\d\\.\\d\\d\\.\\d\\d\\d\\d')";
    private const string COMMENTLINE_REG = "(//.*|--.*)";
    private const string CODE = "([A-ZА-Я_0-9]+)";

    private static bool isNumeric(string teststring)
    {
      int result;
      return int.TryParse(teststring, out result);
    }

    public static IList<ClassificationSpan> GetTokens(SnapshotSpan span, IClassificationTypeRegistryService registry)
    {
      List<string> stringList = new List<string>()
      {
        "Если",
        "If",
        "Тогда",
        "Then",
        "ИначеЕсли",
        "ElsIf",
        "Иначе",
        "Else",
        "КонецЕсли",
        "EndIf",
        "Цикл",
        "Do",
        "Для",
        "For",
        "По",
        "To",
        "Пока",
        "While",
        "КонецЦикла",
        "EndDo",
        "Процедура",
        "Procedure",
        "КонецПроцедуры",
        "EndProcedure",
        "Функция",
        "Function",
        "КонецФункции",
        "EndFunction",
        "Перем",
        "Var",
        "Экспорт",
        "Export",
        "Перейти",
        "Goto",
        "И",
        "And",
        "Или",
        "Or",
        "Не",
        "Not",
        "Знач",
        "Val",
        "Прервать",
        "Break",
        "Продолжить",
        "Continue",
        "Возврат",
        "Return",
        "Контекст",
        "Context",
        "Далее",
        "Forward",
        "Попытка",
        "Try",
        "Исключение",
        "Except",
        "КонецПопытки",
        "EndTry",
        "ВызватьИсключение",
        "Raise",
        "ОписаниеОшибки",
        "GetErrorDescription",
        "ТекущаяИБКод",
        "CurrentIBCode",
        "ТекущаяИБНаименование",
        "CurrentIBDescr",
        "ТекущаяИБСтатус",
        "CurrentIBStatus",
        "ТекущаяИБЦентральная",
        "IsCurrentIBCenter",
        "ИБСозданияОбъекта",
        "BirthIBOfObject",
        "ЦентральнаяИБКод",
        "CentralIBCode",
        "ТекущаяИБТолькоПолучатель",
        "IsCurrentIBRecepientOnly",
        "ФС",
        "FS",
        "ЗагрузитьВнешнююКомпоненту",
        "LoadAddIn",
        "ПодключитьВнешнююКомпоненту",
        "AttachAddIn",
        "СоздатьОбъект",
        "CreateObject",
        "СтатусВозврата",
        "ReturnStatus",
        "РазделительСтраниц",
        "PageBreak",
        "РазделительСтрок",
        "LineBreak",
        "СимволТабуляции",
        "TabSymbol",
        "Перечисление",
        "Enum",
        "Константа",
        "Const",
        "ПланыСчетов",
        "ChartsOfAccounts",
        "ВидыСубконто",
        "SubcontoKinds",
        "ВидРасчета",
        "CalculationKind",
        "ГруппаРасчетов",
        "CalculationGroup",
        "Регистр",
        "Register",
        "Окр",
        "Round",
        "Цел",
        "Int",
        "Мин",
        "Min",
        "Макс",
        "Max",
        "Лог10",
        "Log10",
        "Лог",
        "Ln",
        "СтрДлина",
        "StrLen",
        "ПустаяСтрока",
        "IsBlankString",
        "СокрЛ",
        "TrimL",
        "СокрП",
        "TrimR",
        "СокрЛП",
        "TrimAll",
        "Лев",
        "Left",
        "Прав",
        "Right",
        "Сред",
        "Mid",
        "Найти",
        "Find",
        "СтрЗаменить",
        "StrReplace",
        "СтрЧислоВхождений",
        "StrCountOccur",
        "СтрКоличествоСтрок",
        "StrLineCount",
        "СтрПолучитьСтроку",
        "StrGetLine",
        "Врег",
        "Upper",
        "Нрег",
        "Lower",
        "OemToAnsi",
        "AnsiToOem",
        "Симв",
        "Chr",
        "КодСимв",
        "Asc",
        "РабочаяДата",
        "WorkingDate",
        "ТекущаяДата",
        "CurDate",
        "ДобавитьМесяц",
        "AddMonth",
        "НачМесяца",
        "BegOfMonth",
        "КонМесяца",
        "EndOfMonth",
        "НачКвартала",
        "BegOfQuart",
        "КонКвартала",
        "EndOfQuart",
        "НачГода",
        "BegOfYear",
        "КонГода",
        "EndOfYear",
        "НачНедели",
        "BegOfWeek",
        "КонНедели",
        "EndOfWeek",
        "ДатаГод",
        "GetYear",
        "ДатаМесяц",
        "GetMonth",
        "ДатаЧисло",
        "GetDay",
        "НомерНеделиГода",
        "GetWeekOfYear",
        "НомерДняГода",
        "GetDayOfYear",
        "НомерДняНедели",
        "GetDayOfWeek",
        "ПериодСтр",
        "PeriodStr",
        "НачалоСтандартногоИнтервала",
        "BegOfStandardRange",
        "КонецСтандартногоИнтервала",
        "EndOfStandardRange",
        "ТекущееВремя",
        "CurrentTime",
        "СформироватьПозициюДокумента",
        "MakeDocPosition",
        "РазобратьПозициюДокумента",
        "SplitDocPosition",
        "Дата",
        "Date",
        "Строка",
        "String",
        "Число",
        "Number",
        "Пропись",
        "Spelling",
        "Формат",
        "Format",
        "Шаблон",
        "Template",
        "ФиксШаблон",
        "FixTemplate",
        "ВвестиЗначение",
        "InputValue",
        "ВвестиЧисло",
        "InputNumeric",
        "ВвестиСтроку",
        "InputString",
        "ВвестиДату",
        "InputDate",
        "ВвестиПериод",
        "InputPeriod",
        "ВвестиПеречисление",
        "InputEnum",
        "Вопрос",
        "DoQueryBox",
        "Предупреждение",
        "DoMessageBox",
        "Сообщить",
        "Message",
        "ОчиститьОкноСообщений",
        "ClearMessageWindow",
        "Состояние",
        "Status",
        "Сигнал",
        "Beep",
        "Разм",
        "Dim",
        "ЗаголовокСистемы",
        "SystemCaption",
        "ИмяКомпьютера",
        "ComputerName",
        "ИмяПользователя",
        "UserName",
        "ПолноеИмяПользователя",
        "UserFullName",
        "НазваниеНабораПрав",
        "RightName",
        "ПравоДоступа",
        "AccessRight",
        "НазваниеИнтерфейса",
        "UserInterfaceName",
        "КаталогПользователя",
        "UserDir",
        "КаталогИБ",
        "IBDir",
        "КаталогПрограммы",
        "BinDir",
        "КаталогВременныхФайлов",
        "TempFilesDir",
        "КаталогБазыДанных",
        "DBDir",
        "МонопольныйРежим",
        "ExclusiveMode",
        "ОсновнойЯзык",
        "GeneralLanguage",
        "НачатьТранзакцию",
        "BeginTransaction",
        "ЗафиксироватьТранзакцию",
        "CommitTransaсtion",
        "ОтменитьТранзакцию",
        "RollBackTransaction",
        "ЗначениеВСтрокуВнутр",
        "ValueToStringInternal",
        "ЗначениеИзСтрокиВнутр",
        "ValueFromStringInternal",
        "ЗначениеВСтроку",
        "ValueToString",
        "ЗначениеИзСтроки",
        "ValueFromString",
        "ЗначениеВФайл",
        "ValueToFile",
        "ЗначениеИзФайла",
        "ValueFromFile",
        "СохранитьЗначение",
        "SaveValue",
        "ВосстановитьЗначение",
        "RestoreValue",
        "ПолучитьТА",
        "GetAP",
        "ПолучитьДатуТА",
        "GetDateOfAP",
        "ПолучитьВремяТА",
        "GetTimeOfAP",
        "ПолучитьДокументТА",
        "GetDocOfAP",
        "ПолучитьПозициюТА",
        "GetAPPosition",
        "УстановитьТАна",
        "SetAPToBeg",
        "УстановитьТАпо",
        "SetAPToEnd",
        "РассчитатьРегистрыНа",
        "CalcRegsOnBeg",
        "РассчитатьРегистрыПо",
        "CalcRegsOnEnd",
        "ВыбранныйПланСчетов",
        "DefaultChartOfAccounts",
        "ОсновнойПланСчетов",
        "MainChartOfAccounts",
        "СчетПоКоду",
        "AccountByCode",
        "НачалоПериодаБИ",
        "BeginOfPeriodBT",
        "КонецПериодаБИ",
        "EndOfPeriodBT",
        "КонецРассчитанногоПериодаБИ",
        "EndOfCalculatedPeriodBT",
        "МаксимальноеКоличествоСубконто",
        "MaxSubcontoCount",
        "НазначитьСчет",
        "SetAccount",
        "ВвестиПланСчетов",
        "InputChartOfAccounts",
        "ВвестиВидСубконто",
        "InputSubcontoKind",
        "ОсновнойЖурналРасчетов",
        "BasicCalcJournal",
        "ТипЗначения",
        "ValueType",
        "ТипЗначенияСтр",
        "ValueTypeStr",
        "ПустоеЗначение",
        "EmptyValue",
        "ПолучитьПустоеЗначение",
        "GetEmptyValue",
        "НазначитьВид",
        "SetKind",
        "ПрефиксАвтоНумерации",
        "AutoNumPrefix",
        "ПолучитьЗначенияОтбора",
        "GetSelectionValues",
        "ЗаписьЖурналаРегистрации",
        "LogMessageWrite",
        "КомандаСистемы",
        "System",
        "ЗапуститьПриложение",
        "RunApp",
        "ЗавершитьРаботуСистемы",
        "ExitSystem",
        "НайтиПомеченныеНаУдаление",
        "FindMarkedForDelete",
        "НайтиСсылки",
        "FindReferences",
        "УдалитьОбъекты",
        "DeleteObjects",
        "ОбработкаОжидания",
        "IdleProcessing",
        "ОткрытьФорму",
        "OpenForm",
        "ОткрытьФормуМодально",
        "OpenFormModal",
        "_IdToStr",
        "_StrToID",
        "_GetPerformanceCounter",
        "Календари",
        "Calendars",
        "Метаданные",
        "Metadata",
        "Последовательность",
        "Sequence",
        "ПравилоПерерасчета",
        "RecalculationRule"
      };
      List<ClassificationSpan> classificationSpanList = new List<ClassificationSpan>();
      ITextSnapshotLine containingLine = span.Start.GetContainingLine();
      int position = containingLine.Start.Position;
      string text = containingLine.GetText();
      int val2 = int.MaxValue;
      List<Span> spanList = new List<Span>();
      foreach (Match match in new Regex("(//.*|--.*)").Matches(text))
      {
        val2 = Math.Min(match.Index, val2);
        classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), match.Length), registry.GetClassificationType("OneSComment")));
      }
      foreach (Match match in new Regex("[\"|\\|](\\.|[^\"]|(\"\"))*[\"|\\r\\n]").Matches(text))
      {
        if (match.Index < val2)
        {
          classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), match.Length), registry.GetClassificationType("OneSText")));
          spanList.Add(new Span(match.Index, match.Length));
        }
        else
          break;
      }
      foreach (Match match in new Regex("[\"|\\|](\\.|[^\"]|(\"\"))*").Matches(text))
      {
        if (match.Index < val2)
        {
          int length = match.Length;
          if (match.Index + length >= val2)
            length = val2 - match.Index;
          bool flag = false;
          foreach (Span span1 in spanList)
          {
            if (match.Index >= span1.Start && match.Index <= span1.Start + span1.Length)
              flag = true;
          }
          if (!flag)
          {
            classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), length), registry.GetClassificationType("OneSText")));
            spanList.Add(new Span(match.Index, match.Length));
          }
        }
        else
          break;
      }
      foreach (Match match in new Regex("('\\d\\d\\.\\d\\d\\.\\d\\d\\d\\d')").Matches(text))
      {
        if (match.Index < val2)
        {
          bool flag = false;
          foreach (Span span1 in spanList)
          {
            if (match.Index >= span1.Start && match.Index <= span1.Start + span1.Length)
              flag = true;
          }
          if (!flag)
          {
            classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), match.Length), registry.GetClassificationType("OneSDate")));
            spanList.Add(new Span(match.Index, match.Length));
          }
        }
        else
          break;
      }
      foreach (Match match in new Regex("([A-ZА-Я_0-9]+)", RegexOptions.IgnoreCase).Matches(text))
      {
        if (match.Length != 0)
        {
          if (match.Index < val2)
          {
            bool flag = false;
            foreach (Span span1 in spanList)
            {
              if (match.Index >= span1.Start && match.Index <= span1.Start + span1.Length)
                flag = true;
            }
            if (!flag)
            {
              if (stringList.Contains(match.Value))
                classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), match.Length), registry.GetClassificationType("OneSKeyword")));
              else if (OneSCodeHelper.isNumeric(match.Value))
                classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), match.Length), registry.GetClassificationType("OneSNumber")));
              else
                classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), match.Length), registry.GetClassificationType("OneSError")));
            }
          }
          else
            break;
        }
      }
      foreach (Match match in new Regex("(\\.|\\,|\\;|\\\\|\\(|\\)|\\=|\\+|\\-|\\*|\\/{1}|\\?|<|>|\\[|\\])").Matches(text))
      {
        if (match.Index < val2)
          classificationSpanList.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + position), match.Length), registry.GetClassificationType("OneSKeyword")));
        else
          break;
      }
      return (IList<ClassificationSpan>) classificationSpanList;
    }
  }
}
