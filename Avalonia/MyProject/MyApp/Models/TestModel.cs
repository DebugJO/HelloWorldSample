namespace MyApp.Models;

public class TestModel
{
    
}

/*
 
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:vm="using:MyApp.ViewModels"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:iconPacks="https://github.com/MahApps/IconPacks.Avalonia"
        xmlns:ex="clr-namespace:MyApp.WindowHelper.ThemeHelper"
        mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
        x:Class="MyApp.Views.MainWindow"
        x:DataType="vm:MainWindowViewModel"
        Title="MyApp"
        UseLayoutRounding="True"
        ExtendClientAreaToDecorationsHint="True"
        ExtendClientAreaTitleBarHeightHint="-1"
        ExtendClientAreaChromeHints="NoChrome"
        TransparencyLevelHint="None"
        Background="{DynamicResource SystemRegionBrush}">

    <Design.DataContext>
        <vm:MainWindowViewModel />
    </Design.DataContext>

    <Window.Styles>
        <Style Selector="Window">
            <Setter Property="Padding" Value="0" />
        </Style>
        <Style Selector="Window[WindowState=Maximized]">
            <Setter Property="Padding" Value="{Binding $self.OffScreenMargin}" />
        </Style>
    </Window.Styles>
    
    <Grid RowDefinitions="32,*,24" ColumnDefinitions="*">
        <DockPanel Grid.Row="0" Grid.Column="0" LastChildFill="True" Background="Transparent">
            <StackPanel DockPanel.Dock="Left" Orientation="Horizontal">
                <StackPanel Orientation="Horizontal" Background="Transparent">
                    <Interaction.Behaviors>
                        <EventTriggerBehavior EventName="PointerPressed">
                            <InvokeCommandAction Command="{Binding TitleBarActionCommand}"
                                                 PassEventArgsToCommand="True" />
                        </EventTriggerBehavior>
                    </Interaction.Behaviors>
                    <Image Source="/Assets/avalonia-logo.ico" Margin="8" />
                </StackPanel>
                <Menu FontWeight="Normal">
                    <MenuItem Padding="0" Margin="0">
                        <MenuItem.Header>
                            <AccessText Text="Open(_O)" />
                        </MenuItem.Header>
                        <MenuItem Header="Home">
                            <MenuItem.Icon>
                                <iconPacks:PackIconCodicons Kind="Home" VerticalAlignment="Center" />
                            </MenuItem.Icon>
                        </MenuItem>
                        <MenuItem Header="First Page">
                            <MenuItem.Icon>
                                <iconPacks:PackIconCodicons Kind="Browser" Margin="0 1" />
                            </MenuItem.Icon>
                        </MenuItem>
                        <MenuItem Header="Second Page">
                            <MenuItem.Icon>
                                <iconPacks:PackIconCodicons Kind="Browser" Margin="0 1" />
                            </MenuItem.Icon>
                        </MenuItem>
                        <Separator />
                        <MenuItem Header="Close">
                            <MenuItem.Icon>
                                <iconPacks:PackIconCodicons Kind="Close" Margin="0 2" />
                            </MenuItem.Icon>
                        </MenuItem>
                    </MenuItem>
                    <Separator Width="8" Background="Transparent" Margin="0" />
                    <MenuItem Padding="0" Margin="0">
                        <MenuItem.Header>
                            <AccessText Text="Settings(_S)" />
                        </MenuItem.Header>
                    </MenuItem>
                    <Separator Width="8" Background="Transparent" Margin="0" />
                    <MenuItem Padding="0" Margin="0">
                        <MenuItem.Header>
                            <AccessText Text="About(_A)" />
                        </MenuItem.Header>
                    </MenuItem>
                </Menu>
            </StackPanel>
            <StackPanel DockPanel.Dock="Right" Orientation="Horizontal">
                <Button Content="test" />
            </StackPanel>
            <DockPanel Background="Transparent">
                <Interaction.Behaviors>
                    <EventTriggerBehavior EventName="PointerPressed">
                        <InvokeCommandAction Command="{Binding TitleBarActionCommand}"
                                             PassEventArgsToCommand="True" />
                    </EventTriggerBehavior>
                </Interaction.Behaviors>
            </DockPanel>
        </DockPanel>
        <DockPanel Grid.Row="1" Background="Transparent">
            <TextBlock x:Name="TestText" Text="{Binding Greeting}"
                       HorizontalAlignment="Center" VerticalAlignment="Center" />
            <Button Content="테마변경" Click="Button_OnClick" />

            <CalendarDatePicker Width="100" FontSize="12" Height="28" />

            <StackPanel Spacing="10" Margin="20">

                <Button Classes="HasIcon Primary" ex:AttachedButton.Icon="Home" Width="40" Height="40" />
                <Button Classes="HasIcon Success" Content="확인" />
                <Button Classes="HasIcon Danger" Content="저장" ex:AttachedButton.Icon="Save" />
                <Button Classes="HasIcon Accent" Content="저장" ex:AttachedButton.Icon="Save" />

                <Button Classes="HasIcon Danger" ex:AttachedButton.Icon="Home" Width="40" Height="40" />
                <Button Classes="HasIcon Success" Content="확인" />
                <Button Classes="HasIcon Primary" Content="저장" ex:AttachedButton.Icon="Save" />
                <Button Classes="HasIcon Accent" Content="저장" ex:AttachedButton.Icon="Save" />
                <Button Classes="HasIcon" ex:AttachedButton.Icon="Home" Content="테스트" />


                <StackPanel>
                    <TextBox Watermark="이름을 입력하세요" />
                    <CheckBox Content="자동 로그인" IsChecked="True" />
                    <ProgressBar Value="70" />
                </StackPanel>

                <StackPanel Margin="20" Spacing="10">

                    <!-- 1. 숫자만 입력 가능 -->
                    <TextBlock Text="숫자 전용:" />
                    <TextBox ex:TextBoxInput.InputMode="NumberOnly"
                             Watermark="숫자만 입력하세요" />

                    <!-- 2. 숫자 + 영문 입력 가능 -->
                    <TextBlock Text="숫자/영문 전용:" />
                    <TextBox ex:TextBoxInput.InputMode="AlphaNumeric"
                             Watermark="숫자와 영문만 입력하세요" />

                    <!-- 3. 날짜 형식 입력 가능 -->
                    <TextBlock Text="날짜 전용 (YYYY-MM-DD):" />
                    <TextBox ex:TextBoxInput.InputMode="Date"
                             Watermark="2023-10-25" />

                    <!-- 전화번호 입력 (숫자와 하이픈만 허용, 최대 13자) -->
                    <TextBox ex:TextBoxInput.InputMode="PhoneNumber"
                             Watermark="010-1234-5678" />

                    <!-- IP 주소 입력 (숫자와 마침표만 허용, 최대 15자) -->
                    <TextBox ex:TextBoxInput.InputMode="IpAddress"
                             Watermark="192.168.0.1" />

                </StackPanel>
            </StackPanel>
        </DockPanel>
        <DockPanel Grid.Row="2" Background="Aqua">
        </DockPanel>
    </Grid>
</Window>
*/