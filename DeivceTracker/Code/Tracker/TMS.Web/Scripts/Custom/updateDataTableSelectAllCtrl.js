//Updates "Select all" control in a data table
function updateDataTableSelectAllCtrl(table) {
    var $table = table.table().container();
    var $chkboxAll = $('tbody input[type="checkbox"]', $table);
    var $chkboxChecked = $('tbody input[type="checkbox"]:checked', $table);
    var chkboxSelectAll = $('thead input[type="checkbox"]', $table).get(0);

    // If none of the checkboxes are checked
    if ($chkboxChecked === undefined || $chkboxChecked.length === 0) {
        chkboxSelectAll.checked = false;
        if ('indeterminate' in chkboxSelectAll) {
            chkboxSelectAll.indeterminate = false;
        }

        // If all of the checkboxes are checked
    } else if ($chkboxChecked.length === $chkboxAll.length) {
        chkboxSelectAll.checked = true;
        if ('indeterminate' in chkboxSelectAll) {
            chkboxSelectAll.indeterminate = false;
        }

        // If some of the checkboxes are checked
    } else {
        chkboxSelectAll.checked = true;
        if ('indeterminate' in chkboxSelectAll) {
            chkboxSelectAll.indeterminate = true;
        }
    }
}