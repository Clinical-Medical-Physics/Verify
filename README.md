# Verify
## SRS Cone MU Verification
Verify MU for SRS Cone plan using ESAPI.

## Background
Verifying SRS cone plan MUs automatically has been slow going. None of the commercial solutions I have tried have been straight forward to setup and use.
In my clinic I developed an Excel spreadsheet to perform the second check. In that sheet, the TMR is interpolated based on the averge depth printed by the
dosimetry report in Cone Planning (part of the Eclipse planning suite). The Cone Factor is read from a table based on the Energy and cone size used in the plan.
The problem with the Excel sheet is that nearly all data had to be entered by hand. Enter ESAPI.

In this program, we pull the TMRs and Cone factors from CDC algorithm processed files on the DCF server. The data are processed from the measured data entered
during CDC commissioning. The cone factor is converted to match the cone factor in the Dosimetry Report from CDC. All other parameters are pulled from the beam.

## TODO
- [✅] Update for newer versions. Currently written for V11.
- [ ] Add/finish building the option to create a model based on CSV filed instead of copying from DCF server.
- [ ] Add plan checks like:
   - [ ] Is max point inside PTV/GTV
   - [ ] Is Isocenter inside PTV/GTV
   - [ ] Correct Energy and Dose Rate selected
   - [ ] Color coded pass/fail
   - [ ] etc.
- [ ] Add beams eye view for each field showing that cone covers the PTV/GTV
