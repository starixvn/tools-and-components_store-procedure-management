var actionSource = ["A", "D", "E", "Nested"];
var statusSource = ["In Progress", "Done", "UAT", "UAT [_xtest]", "UAT [_xpre]", "Not Required", "Released", "Pending"];

$(document).ready(function () {
    var arrayServer = new Array();
    var arrayDatabase = new Array();
    var arrayServerDatabase = new Array();
    var arrayDeveloper = new Array();
    var arrayStoredProcedureIssue = new Array();
    var selectedServerName = "";
    var currentUser = new Object();
    var selectLimit = 20;

    // Parsing parameters
    (window.onpopstate = function () {
        var match,
	        pl = /\+/g,  // Regex for replacing addition symbol with a space
	        search = /([^&=]+)=?([^&]*)/g,
	        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
	        query = window.location.search.substring(1);

        httpQueries = {};
        while (match = search.exec(query))
            httpQueries[decode(match[1])] = decode(match[2]);
    })();

    $.ajax({
        url: 'http://localhost:62215/api/server',
        async: false,
        success: function (response) {
            $.each(response, function (i, data) {
                arrayServer[i] = data;
            });
        }
    });

    $.ajax({
        url: 'http://localhost:62215/api/database',
        async: false,
        success: function (response) {
            $.each(response, function (i, data) {
                arrayDatabase[i] = data;
            });
        }
    });

    $.ajax({
        url: 'http://localhost:62215/api/user',
        async: false,
        success: function (response) {
            $.each(response, function (i, data) {
                arrayDeveloper[i] = data;
            });
        }
    });

    $.ajax({
        url: 'http://localhost:62215/api/StoreProcedure?id=' + httpQueries.issueId + '&includeChild=true',
        async: false,
        success: function (response) {
            $.each(response, function (index, data) {
                arrayStoredProcedureIssue[index] = data;
            });
        }
    });

    currentUser = getAuthor(arrayDeveloper, httpQueries.author);

    var hiddenColumnRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        td.hidden = true;
        td.innerText = value;
        return td;
    };

    var columnSetting = [
                {
                    type: "autocomplete",
                    data: "StoreProcedure.Server.Name",
                    source: convertToSourceName(arrayServer),
                    strict: true
                },
                {
                    type: "autocomplete",
                    data: "StoreProcedure.Database.Name",
                    source: function (query, process) {
                        // Filter database by serverId
                        var instance = $('#storeprocedures').handsontable('getInstance');
                        var pos = instance.getSelected();
                        selectedServerName = instance.getDataAtCell(pos[0], 0);

                        arrayServerDatabase = getDatabasesByServer(arrayDatabase, selectedServerName);
                        filterSelect(arrayServerDatabase, query, process);
                    },
                    strict: true
                },
                {
                    data: "StoreProcedure.Name"
                },
                {
                    type: "checkbox",
                    data: "IsFunction"
                },
                {
                    type: "autocomplete",
                    data: "ActionName",
                    source: actionSource,
                    strict: true
                },
                {
                    type: "autocomplete",
                    data: "Developer.Username",
                    source: function (query, process) {
                        var array = new Array();
                        $.each(arrayDeveloper, function (index, data) {
                            array[index] = data.Username;
                        });
                        filterSelect(array, query, process);
                    },
                    strict: true
                },
                {
                    type: "autocomplete",
                    data: "StatusName",
                    source: statusSource,
                    strict: true
                },
                { data: "Description" },
                { data: "Modifier.Username", readOnly: true}];

    $("#storeprocedures").handsontable({
        data: arrayStoredProcedureIssue,
        startRows: 0,
        startCols: 9,
        rowHeaders: true,
        columnSorting: true,
        colWidths: [130, 130, 310, 60, 70, 120, 100, 100, 100],
        colHeaders: function (col) {
            switch (col) {
                case 0:
                    return "<b>Server</b>";
                case 1:
                    return "<b>Database</b>";
                case 2:
                    return "<b>SP/Function</b>";
                case 3:
                    return "<b>Function</b>";
                case 4:
                    return "<b>Action</b>";
                case 5:
                    return "<b>Developer</b>";
                case 6:
                    return "<b>Status</b>";
                case 7:
                    return "<b>Description</b>";
                case 8:
                    return "<b>Modified By</b>";
                default:
                    return "";
            }
        },
        columns: columnSetting,
        minSpareRows: 1,
        fillHandle: true,
        contextMenu: {
            items: {
                "row_above": {
                    disabled: function () {
                        //if first row, disable this option
                        return ($("#storeprocedures").handsontable('getSelected')[0] === 0);
                    }
                },
                "row_below": {},
                "hsep1": "---------",
                "remove_row": {
                    name: 'Remove this row, ok?',
                    disabled: function () {
                        //if first row, disable this option
                        return ($("#storeprocedures").handsontable('getSelected')[0] === 0);
                    }
                },
                "hsep2": "---------"
            }
        },
        onChange: function (e, changes, source) {
            if (e != null) {
                var instance = $('#storeprocedures').handsontable('getInstance');
                var pos = instance.getSelected();

                if (e[0][1] == "StoreProcedure.Server.Name") {
                    selectedServerName = e[0][3];

                    arrayServerDatabase = getDatabasesByServer(arrayDatabase, selectedServerName);

                    instance.setDataAtCell(pos[0], 1, arrayServerDatabase[0], arrayServerDatabase);
                }

                if (e[0][1] != "Modifier.Username") {
                    instance.setDataAtCell(pos[0], 8, currentUser.Username);
                }
            }
        },
        afterChange: function (source, change) {
            if (change === 'loadData' || source[0][1] === 'Modifier.Username') {
                return;
            }
            var instance = $('#storeprocedures').handsontable('getInstance');
            saveStoredProcedureIssue(instance, source[0][0]);
        }
    });

    function saveStoredProcedureIssue(instance, row) {
        if (!isEmptyRow(instance, row)) {
            var rowData = instance.getDataAtRow(row);
            var serverId = _.find(arrayServer, function (item) {
                return item.Name === rowData.StoreProcedure.Server.Name;
            }).Id;

            var databaseId = _.find(arrayDatabase, function (item) {
                return item.Name === rowData.StoreProcedure.Database.Name && item.Server.Id === serverId;
            }).Id;

            var developerId = _.find(arrayDeveloper, function (item) {
                return item.Username === rowData.Developer.Username;
            }).Id;

            console.log(rowData);
            var param = [
			    'id=' + (rowData.Id || 0),
			    'serverId=' + (serverId || 0),
			    'databaseId=' + (databaseId || 0),
			    'name=' + $.trim(rowData.StoreProcedure.Name || ""),
			    'isFunction=' + (rowData.IsFunction || false),
			    'actionName=' + $.trim(rowData.ActionName || ""),
			    'modifiedById=' + currentUser.Id,
			    'status=' + convertToStatusId(rowData.StatusName),
			    'description=' + $.trim(rowData.Description || ""),
			    'issueId=' + httpQueries.issueId,
			    'developerId=' + (developerId || 0),
                'storedProcedureId=' + (rowData.StoreProcedure.Id || 0),
                'isAddNew=true'];

            console.log(param);
            $.ajax({
                url: 'http://localhost:62215/api/storeprocedure?' + param.join('&'),
                type: 'POST',
                success: function (response) {

                }
            });


        }
    };

    function convertToSourceName(values) {
        var array = new Array();
        $.each(values, function (index, data) {
            array[index] = data.Name;
        });

        return array;
    };

    function getDatabasesByServer(value, serverName) {
        var array = new Array();
        $.each(value, function (index, data) {
            if (serverName === data.Server.Name) {
                array.push(data.Name);
            }
        });

        return array;
    }

    function filterSelect(array, q, p) {
        var regex = new RegExp(q, "i");
        var found = _.filter(array, function (e) {

            return regex.test(e);
        });

        var limit = found.slice(0, selectLimit - 1);
        if (found.length > selectLimit) {
            limit.push("...");
        };

        p(limit);
    };

    function getAuthor(array, author) {
        return (_.find(array, function (dev) {
            return dev.Username === atob(author);
        }) || { Username: "" });
    }

    function isEmptyRow(instance, row) {
        var rowData = instance.getDataAtRow(row);
        if (rowData.StoreProcedure.Server.Name != null
            && rowData.StoreProcedure.Database.Name != null
            && rowData.StoreProcedure.Name != null
            && rowData.ActionName != null
            && rowData.Developer.Username != null
            && rowData.StatusName != null) {
            return false;
        }

        return true;
    }

    function convertToStatusId(name) {
        var statusId = 0;

        switch (name) {
            case "In Progress":
                statusId = 1;
                break;
            case "Done":
                statusId = 2;
                break;
            case "UAT [_xtest]":
                statusId = 3;
                break;
            case "UAT [_xpre]":
                statusId = 4;
                break;
            case "Not required":
                statusId = 5;
                break;
            case "Released":
                statusId = 6;
                break;
            case "Pending":
                statusId = 7;
                break;
            case "UAT":
                statusId = 8;
                break;
            default:
                statusId = -1;
                break;
        }

        return statusId;
    }
});