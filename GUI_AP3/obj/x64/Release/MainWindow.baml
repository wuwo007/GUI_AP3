   M S B A M L   `   `   `  ����   GUI_AP3    GUI_AP3.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$	  AP3��$	��550��$	��800�� � Icon$ ./Resources/AP3.ico��.��]� .+�o� $	��Top=�$
��Left=�.@�m� -   $��MenuFile��$��_File��.@�m� -   $��Load parameters��m� -   $��Save parameters��m� -   $��_Exit�� � ToolTip$ Exit the application��m� -   $��MenuInitial��$��_Initial Test��.@�m� -   $��Environment test��m� -   $��menuHelp��$��_Help��.@�m� -   $��	About AP3��m� -	   $��Online website��m� -
   $��Email us���� -   $��	statusBar��$��20��$��Bottom=�.@��� -    ��Name$ statBarText��$
��Wrap=�$��Ready�� J� OpacityMask �	#FFCA3131� $	��Top=� � RowDefinitions �� $��* ��� $��* ��� $��4* ��� $��* ��� $��* �.+�� $��0�� � ColumnDefinitions �� $
��1.1* ��� $��2* ��� $��* �.+��� $��0��
���ؿ�� �� CornerRadius$ 10,10,10,10��$��1q�$��Center=�$��Center=�._��� $��Trained model ��$��Center=�$��Center=� �� .+��� 97Please choose the trained model according to the input.�� -   $��SpeciesComboBox��$��1�� �� SelectedValuePath$ Content��$��Center=�$��Center=�	 �� SelectedIndex$	 0��.@��� 
 �� 
IsSelected
 . $��Italic$�.��Saccharomyces cerevisiae�� $��Italic$�.��Mus�� $��Italic$�.��Homo sapiens�� $��Italic$�.��E.coli�� .��Mix� $��1�� �� $
��1.1* ��� $��* ��� $��* ��� $��* �.+��� $��0��
���ؿ��$��1q�$ 10,10,10,10��$��Center=�$��Center=�._��� $��Identifier parsing rule��$��Center=�$��Center=� �� .+��� $
��Wrap=�TRThe regular expression is used to extract protein identifiers from the fasta file.�� -   $��ParseRuleTextBox��$��1��
���   �$��1q�$��Center=�$��Center=�$	��100��$��28��$��0,4,0,4q� �� TextWrapping$
 Wrap=�.���� -   $��TestRuleButton��$
��Test��$��2��$��Center=�$��Center=��� $��2��.���  �� $��* ��� $��* �� $��2�� �� $
��1.1* ��� $��* �.+��� $��0��$��Input�� �� .+��� $
��Wrap=���The input style should be FASTA Format.This textbox will show the first 100 proteins. The user can paste the sequences into this textbox directly or load from file..��]� .+��� -   $��FollowSequencesCheckbox��$	��Top=�$(��"Just input the following sequences��$��10,2,3,2q�$
��Left=�$��Center=��� -   $��FileInputCheckbox��$	��Top=�$(��"Load input from the following file��$��10,2,3,2q�$
��Left=� d� 	IsChecked$
 TrueY�$��Center=�� $	��Top=� �� $��* ��� $
��Auto �.+��� -   $��InputFilePathTextBox��$��0��
���   �$
 Wrap=�$
��auto��" �� VerticalScrollBarVisibility$
 Auto=�$ �� HorizontalScrollBarVisibility$
 Auto=�$��0,0,3,3q� �� AcceptsReturn . �� -    ��Name$ InputDirectory_Browse��$��1��$��Browse��$��Right=�$��Center=�$��5,0,0,3q��� -   $��InputTextBox��
���   �$��1q�$
 Wrap=�$
 Auto=�$
 Auto=� . �� $��1��$��In-silico digestion��.���  �� $��2* ��� $��* ��� $��* � �� $��3* ��� $��1* �.+��� $��0��$��0��
���ؿ��$��1q�$ 10,10,10,10��$��Center=�$��Center=�._��� .���� $
��Wrap=�)'Maximum number of missed cleavage sites�� -   $��MaxMissCleavage��$��0��$��1��$��2��$��20��$��20��$��Center=�$��Center=�$��15(�
���   �$��1q�.���� $��1��$��0��
���ؿ��$��1q�$ 10,10,10,10��$��Center=�$��Center=�._��� $)��#Minimum length of digested peptides���� -   $��MinPepLength��$��1��$��1��$��Center=�$��20��$��20��$��Center=�$��15(�$��7��
���   �$��1q��� $��2��$��0��
���ؿ��$��1q�$ 10,10,10,10��$��Center=�$��Center=�._��� $)��#Maximum length of digested peptides���� -   $��MaxPepLength��$��2��$��1��$��Center=�$��20��$��20��$��Center=�$��15(�$��40��
���   �$��1q�� $��3�� �� $
��1.1* ��� $��3* �.+��� $��0��
���ؿ��$��1q�$ 10,10,10,10��$��Center=�$��Center=�._��� $��Result directory��$��Center=�$��Center=��� -   $��ResultPathTextBox��$��1��
���   �$��1q�$
 Wrap=�$��Center=�$��30��$	��200��$��	13,4,10,2q�]� $��5��.+��� -   $��AP3RunButton��$��Run AP3��$��Center=�$��Center=�