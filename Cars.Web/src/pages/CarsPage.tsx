import { useEffect, useState } from 'react';
import CarService from '../services/CarService';
import { GetCarRespone, SaveCarRequest } from '../types/CarRequest';
import Table from '../components/Table/Table';
import dayjs from 'dayjs';
import { BaseGetResponse } from '../types/BaseRequest';
import Modal from '../components/Modal/Modal';
import Input from '../components/Input/Input';
import BrandsSelect from '../components/Select/BrandsSelect';
import BodyTypesSelect from '../components/Select/BodyTypesSelect';
import Notification from '../components/Notification/Notification';

/** Страница машин */
const CarsPage = () => {
    const [carData, setCars] = useState<BaseGetResponse<GetCarRespone>>();
    const [pageNumber, setPageNumber] = useState(1);
    const [editModel, setEditModel] = useState<Partial<SaveCarRequest>>();
    const [errorState, setErrorState] = useState<string>();
    const [successMessage, setSuccessMessage] = useState<string | null>(null);

    const fetchData = () => {
        CarService.GetListAsync({
            pageNumber,
            pageSize: 10,
        })
            .then(response => setCars({
                ...response,
                entities: response.entities.map(x => ({ ...x, createdOn: dayjs(x.createdOn).format('DD.MM.YYYY HH:mm') }))
            }))
            .catch((e) => console.warn('Ошибка при получении данных', e));
    }

    useEffect(() => {
        fetchData();
    }, [pageNumber]);

    const onEdit = async (index: number) => {
        setEditModel(carData?.entities[index]);
    };

    const onDelete = async (index: number) => {
        const item = carData?.entities[index];

        if (!item?.id) {
            return;
        }

        try {
            await CarService.DeleteAsync(item.id);
            setSuccessMessage('Успешное удаление');
            fetchData();
        }
        catch (error: any) {
            const text = error?.response?.data?.title ?? "Ошибка при удалении данных";
            setErrorState(text);
        }
    }

    const onSave = async () => {
        if (!editModel) {
            return;
        }

        if (!editModel.name) {
            setErrorState("Необходимо заполнить наименование");
            return;
        }
        if (!editModel.brandId) {
            setErrorState("Необходимо заполнить бренд");
            return;
        }
        if (!editModel.seatsCount) {
            setErrorState("Необходимо заполнить число сидений");
            return;
        }
        if (editModel.seatsCount < 1 || editModel.seatsCount > 12) {
            setErrorState("Число сидений должно быть от 1 до 12");
            return;
        }
        if (!editModel.bodyTypeId) {
            setErrorState("Необходимо заполнить тип кузова");
            return;
        }
        if (editModel?.url != null && !editModel.url.endsWith('.ru')) {
            setErrorState("Сайт должен быть в домене \".ru\"");
            return;
        }

        try {
            if (editModel.id) {
                await CarService.UpdateAsync(editModel.id, editModel as SaveCarRequest);
            }
            else {
                await CarService.CreateAsync(editModel as SaveCarRequest);
            }

            setSuccessMessage('Успешное сохранение');
            setEditModel(undefined);
            fetchData();
        }
        catch (error: any) {
            const text = error?.response?.data?.title ?? "Ошибка";
            setErrorState(text);
        }
    };

    return (
        <div>
            <button onClick={() => setEditModel({})}>Добавить</button>
            {successMessage && (
                <Notification
                    message={successMessage}
                    onClose={() => setSuccessMessage(null)}
                />
            )}
            {editModel && <Modal onClose={() => setEditModel(undefined)}>
                <div>{editModel?.id ? 'Редактирование' : "Добавление"} машины</div>
                <Input
                    label='Наименование'
                    value={editModel?.name}
                    onChange={(v) => setEditModel({ ...editModel, name: v as string })}
                />
                <BrandsSelect
                    id={editModel?.brandId}
                    onChange={(v) => setEditModel({ ...editModel, brandId: v as string })}
                />
                <Input
                    label='Число мест'
                    value={editModel?.seatsCount}
                    onChange={(v) => setEditModel({ ...editModel, seatsCount: +v })}
                    type='number'
                />
                <BodyTypesSelect
                    id={editModel?.bodyTypeId}
                    onChange={(v) => setEditModel({ ...editModel, bodyTypeId: v as string })}
                />
                <Input
                    label='Url'
                    value={editModel?.url}
                    onChange={(v) => setEditModel({ ...editModel, url: v as string })}
                />
                <button onClick={onSave}>Сохранить</button>
            </Modal>}
            {errorState && <Modal onClose={() => setErrorState(undefined)}>
                {errorState}
            </Modal>}
            <Table
                currentPage={pageNumber}
                onChangePage={setPageNumber}
                totalCount={carData?.totalCount ?? 0}
                data={carData?.entities ?? []}
                columns={[
                    {
                        header: '',
                        key: 'Id',
                        customCell: (index: number) => <div style={{ cursor: 'pointer' }} onClick={() => onEdit(index)}>Редактировать</div>
                    },
                    {
                        header: "Наименование",
                        key: "name"
                    },
                    {
                        header: "Бренд",
                        key: "brandName"
                    },
                    {
                        header: "Тип кузова",
                        key: "bodyTypeName"
                    },
                    {
                        header: "Число мест",
                        key: "seatsCount"
                    },
                    {
                        header: "Официальный сайт дилера",
                        key: "url",
                        customCell: (index: number) => <a target="_blank" href={
                            carData?.entities[index].url?.startsWith('http')
                                ? carData?.entities[index].url
                                : `http://${carData?.entities[index].url}`}>{carData?.entities[index].url}</a>
                    },
                    {
                        header: "Дата создания",
                        key: "createdOn"
                    },
                    {
                        header: '',
                        key: 'Id',
                        customCell: (index: number) => <div style={{ cursor: 'pointer' }} onClick={() => onDelete(index)}>Удалить</div>
                    },
                ]}
            />
        </div>
    );
};

export default CarsPage;