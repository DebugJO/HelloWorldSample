```xaml
<ListView ItemsSource={Binding MyItems}>
    <ListView.View>
        <GridView>
            <!-- declare a GridViewColumn for each property -->
        </GridView>
    </ListView.View>
    <ListView.ItemContainerStyle>
        <Style TargetType="ListViewItem">
            <EventSetter Event="PreviewMouseLeftButtonDown" Handler="ListViewItem_PreviewMouseLeftButtonDown" />
        </Style>
    </ListView.ItemContainerStyle>
</ListView>
```

```cs
private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
{
    var item = sender as ListViewItem;
    if (item != null && item.IsSelected)
    {
        //Do your stuff
    }
}
```
