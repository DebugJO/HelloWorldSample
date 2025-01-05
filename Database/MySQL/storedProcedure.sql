CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AddInOutVehicle`(
    IN pVehicleNumber VARCHAR(20), 
    IN pLprID VARCHAR(10), 
    IN pInOutDateTime VARCHAR(20), 
    IN pVehicleType VARCHAR(10), 
    OUT oErrorCode VARCHAR(255), 
    OUT oErrorMessage VARCHAR(255), 
    OUT oResult INT)
    COMMENT '입출차리스트입력'
begin 
  declare seq varchar(17);
  declare image varchar(100);

  declare exit handler for sqlexception, sqlwarning, not found 
  begin
    get diagnostics condition 1  oErrorCode = mysql_errno, oErrorMessage = message_text;
    set oResult = 0;
    rollback;
  end;

  start transaction;

    set seq = fn_GetVehicleSEQ();
    set image = concat(left(seq, 4), '-', left(seq, 6), '-', left(seq, 8), '-', seq, '.jpg');

    insert into invehicle (VehicleNumberSEQ, VehicleNumber, LprID, InOutType, InOutDateTime, VehicleType, VehicleImage)
      values (seq, pVehicleNumber, pLprID, 'IN', pInOutDateTime, pVehicleType, image);

    insert into outvehicle (VehicleNumberSEQ, VehicleNumber, LprID, InOutType, InOutDateTime, VehicleType, VehicleImage, UpdateYN)
      values (seq, pVehicleNumber, pLprID, 'OUT', pInOutDateTime, pVehicleType, image, 'N');     

  commit;
  set oResult = 1;
end
