   M S B A M L   `   `   `  ????   Note    Note.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     ? Title$
  Note??$	??800??$	??550??$	??650??$	??400?? J? 	AllowDrop .   ? Icon$ /Imgs/Icon.png?? ? WindowStartupLocation$ CenterScreen=?'  !System.Windows.Shell.WindowChrome   WindowChrome      CaptionHeight$ 0Q?.??? -    ?Name$ OwnerScreen??   
BackWindow#???    ? RowDefinitions ?? $??24 ??? $??34 ???  ? ColumnDefinitions ?? $	??250 ??? .+??? $	??250??  AnotherSPanel#???  	 ?? Orientation$	 
Horizontal=?.+??? -     BtnExit#???  .????   Exit#???  ?? -   .????   
Fullscreen#???  ?? -   .????   Minimize#???  ?? $	??154??$??SF Pro Display)?$??Center=?
 ?? TextAlignment$
 Center=?$
??Note???? -   .????   Settings#???  ?? $	??110??$??14??$??1??$??Center=?$	 
Horizontal=??? $??Bottom=??? $??1??$??Bottom=??? $??1??._??? #???  $	 
Horizontal=?.+??? -   $??7 0 0 0q?$??14??$??14??.????  
 AddImg#???  ?? -    ??Name$ Search??$	??5 0q?$	??216??$??18??$??10(?$	??0.5q? 
 Search#???  $??Поиск...???? -	    ??Name$ NoteList??$??2??$??0q?$??Hidden=????? .R??? $	??240??$??54?? ? ContextMenu ?? .@?m? -
   ???? 
???   ?$ ??Переименовать??.+??? -   $
??10 5q?$
??Left=?.+??? $	??150??$??SF Pro Display)?$??14(?$
??Left=?$??CharacterEllipsis=????? *Name+?? $??29??$	??224??$??SF Pro Display)?$
??Left=?$??CharacterEllipsis=?$
??Wrap=????? *Description+?? -    ??Name$ 
ContBtnDel??$??2??$
??auto?? 	 BtnCont#??? 	 $??	0 0.5 0 0q?$??Bottom=?$y?Hidden=?._??? -    ??Name$ BtnDel??$
??auto??{?.  $??Удалить???? -    ??Name$ Editor??$??20 15q?$??1??$??1??$??2??
?????? $??0q?$??SF Pro Display)?$y?Hidden=??? $??2??$??Bottom=??? $??3??$??Right=? ? LayoutTransform ??  ?? Angle$ 90Q??? -   $ MoreCtx??$??226 24q?$??2??$??3??$	??100??$
??Left=?$	??Top=?$	??0.5q?$y?Hidden=? 
 MoreCtx#??? 
 ._??? $	??100??.+??? $
??0.25q?._??? $	??100??.????  ?? Padding$
 10 0q?$	??100??$??Импорт??$
??Left=??? $
??0.25q?._??? -   $	??100??.???? $
 10 0q?$	??100??$??Удалить??$
??Left=??? $
??0.25q?._??? -   $	??100??.???? $
 10 0q?$	??100??$??Настройки??$
??Left=??? $
??0.25q?._??? -   $	??100??.???? $
 10 0q?$	??100??$??О программе??$
??Left=?? -   $ BackDiag??$??3??$??2??
???   ?$y?Hidden=?? -   $ 	RenameDoc??$??3??$??2??$y?Hidden=?.+??? $	??300??$	??150??#???  .+??? $??5q?$??SF Pro Display)?$??Black"?$
 Center=?$/??)Переименовать заметку???? ?? -   $ FileName??$??30q?$	??200??$??20??$??12(?$	??Top=??? $	??100??$	??0.5q?._??? -   $	??100??$ ??Переименовать??? -   $ 	CreateDoc??$??3??$??2??$y?Hidden=?.+??? $	??300??$	??150??#???  .+??? $??5q?$??SF Pro Display)?$??Black"?$
 Center=?$#??Создать заметку???? ?? -   $ NewFileName??$??30q?$	??200??$??20??$??12(?$	??Top=??? $	??100??$	??0.5q?._??? -   $	??100??$??Создать??? -   $ Settings??$??3??$??2??$y?Hidden=?.+??? $	??300??$	??150??#???  .+??? $??5q?$??SF Pro Display)?$??Black"?$	??Top=?$
 Center=?$??Настройки???? ?? -    ??Name$ CmbTheme??$
??0 30q?$	??200??$??20??$??Center=? ?? DisplayMemberPath$
 Name??? -   $ AppInfo??$??3??$??2??$y?Hidden=?.+??? $	??300??$	??150??#???  .+??? $??5q?$??SF Pro Display)?$??Black"?$
 Center=?$??О программе???? ?? $??30q?$??20??$??12(?$??Center=?$)??#Текущая версия - v0.11???? $	??100??$	??0.5q?._??? -   $	??100??$??Обновить??? -   $ 	DragNDrop??$??3??$??2?? . $y?Hidden=?
???   ?