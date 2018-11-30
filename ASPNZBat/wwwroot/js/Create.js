
// http://jsfiddle.net/8L7v0pbp/
//LOOK  AT _STATS IN SHARED TO SEE THE TABLE *@

var maxseats = 5; //max bookings for a session - disables checkbox above that number
var threeqseats = 4; //some middle range number set at 3 quarters but wherever you like
var oneqseats = 2; //empty or lower level number not use?

$(document).ready(function () {
    $('td.sess').each(function () {
        var diff = parseInt($(this).html());
        var statID = $(this).attr("id"); //get the ID   <td id="sS1"
        var sessID = statID.substring(1); //strip out the extra s to ge the session ID

        //  alert(statID); // just a test

        if (diff < threeqseats) {
            $(this).css('background', 'rgb(170, 218, 170)');

        } else if (diff > maxseats) {
            $(this).css('background', 'rgb(239, 135, 135)');
            //document.getElementById(sessID).enabled = false;
            //document.getElementById(s1L).innerHTML = "Test";

        } else if (diff <= maxseats && diff >= threeqseats) {
            $(this).css('background', 'rgb(233, 233, 45)');
        }

    });





});

//RADIO BUTTON SELECTION
var DateSelected;

//The change event on the radio button for the date
$(document).ready(function () {
    $('input[type="radio"]').on("change",
        function (e) {
            e.preventDefault(); //catch any errors
            //get the value of the checked radio button - we want the date
            DateSelected = $('input[name=SeatDate]:checked').val();

            ////---------------------

            //SETTING THE LABEL COLOR AND ENABLED
            $('#tablestats tbody tr').each(function () {
                //get the first cell at 0
                var id = $(this).find("td").eq(0).html();
                //  alert(id + " " + DateSelected);
                if (id === DateSelected) {
                    // alert(DateSelected + " Hit it! " + id);

                    //use map to get an array of cells hold all the data in the td cells
                    var tabledata = $(this).children("td").map(function () {
                        return $(this).text();
                    }
                    ).get();
                    var data = DateSelected + " ";
                    var lblid = "#S1L"; //create a session label for the session
                    var cbxid = "#S1"; //create a checkbox ID

                    //build for 16 sessions
                    for (var i = 0; i < 16; i++) {
                        data += tabledata[i] + " "; //get the table data at the index
                        lblid = "#S" + i + "L"; //build the label
                        cbxid = "#S" + i;  //build the checkbox

                        //give colors to the css based on tabledata number
                        if (tabledata[i] < threeqseats) {
                            $(lblid).css("background-color", 'rgb(170, 218, 170)');
                            $(cbxid).attr('disabled', false);
                        } else if (tabledata[i] > maxseats) {
                            $(lblid).css("background-color", 'rgb(239, 135, 135)');
                            $(cbxid).attr('disabled', true); //disable the checkboxes if over the limit
                        } else if (tabledata[i] <= maxseats && tabledata[i] >= threeqseats) {
                            $(lblid).css("background-color", 'rgb(233, 233, 45)');
                            $(cbxid).attr('disabled', false);
                        }
                    }
                    //  alert(data);
                    //https://forums.asp.net/t/2061858.aspx?Change+label+back+color+in+JQuery
                    //$("#S1L").css("background-color", "Red");
                    // $("#S1L").prop("enabled", false);
                }
            });

        });
});
//click event for table
$(document).ready(function () {
    $("#tablestats tbody tr").click(function () {
        //use map to get an array of cells
        var tabledata = $(this).children("td").map(function () {
            return $(this).text();
        }
        ).get();
        var data = DateSelected + " ";

        for (var i = 0; i < 16; i++) {
            data += tabledata[i] + " ";
        }
        alert(data);


    });

});